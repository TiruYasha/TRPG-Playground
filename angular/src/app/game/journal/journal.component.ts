import { Component, OnInit, Input } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalService } from './journal.service';
import { CreateFolderDialogComponent } from './create-folder-dialog/create-folder-dialog.component';
import { MatDialog } from '@angular/material';
import { AddJournalFolderRequestModel } from 'src/app/models/journal/requests/AddJournalFolderRequest.model';
import { Guid } from 'src/app/utilities/guid.util';
import { AddedJournalFolderModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { TreeTraversal } from 'src/app/utilities/tree-traversal.util';



@Component({
  selector: 'trpg-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent implements OnInit {
  @Input() isOwner: boolean;
  @Input() gameId: string;

  journalItems: JournalItem[] = [];

  subIcons = ['create_new_folder', 'person_add', 'note_add'];

  treeControl = new NestedTreeControl<JournalItem>((node: JournalFolder) => node.journalItems);
  dataSource = new MatTreeNestedDataSource<JournalItem>();

  hasChild = (_: number, node: JournalFolder) => !!node.journalItems;

  constructor(private journalService: JournalService, public dialog: MatDialog) {
    this.dataSource.data = this.journalItems;
  }

  ngOnInit() {
    this.journalService.setup(this.gameId);

    this.journalService.AddedJournalFolder.subscribe((model: AddedJournalFolderModel) => this.addFolderToJournalItems(model));
    this.journalService.AddedToGroup.subscribe((data: JournalItem[]) => {
      this.journalItems = data;
      this.refreshDataSource();
    });
  }

  subIconClicked(icon: string) {
    if (icon === 'create_new_folder') {
      this.openCreateNewFolderDialog();
    }
  }

  addFolderToJournalItems(model: AddedJournalFolderModel): void {
    const folder = new JournalFolder();
    folder.name = model.name;
    folder.id = model.id;

    if (model.parentId !== Guid.getEmptyGuid()) {
      const child = this.findChild(model);
      child.journalItems.push(folder);
    } else {
      this.journalItems.push(folder);
    }

    this.refreshDataSource();
  }

  addJournalFolderToParent(parentFolder: JournalFolder) {
    this.openCreateNewFolderDialog(parentFolder.id);
  }

  clickFolder(node: JournalItem) {
    this.treeControl.toggle(node);
  }

  private findChild(model: AddedJournalFolderModel) {
    return TreeTraversal.findChild(model.parentId, this.journalItems,
      (item: JournalItem) => (item as JournalFolder).journalItems, (id: string, item: JournalItem) => id === item.id) as JournalFolder;
  }

  private refreshDataSource() {
    this.dataSource.data = null;
    this.dataSource.data = this.journalItems;
  }

  private openCreateNewFolderDialog(parentFolderId: string = null) {
    const dialogRef = this.dialog.open(CreateFolderDialogComponent, {
      width: '250px',
      data: { name: '' }
    });

    dialogRef.afterClosed().subscribe((folderName) => this.createNewFolder(folderName, parentFolderId));
  }

  private createNewFolder(folderName: string, parentFolderId: string) {
    if (folderName === void 0) {
      return;
    }

    const folderRequest: AddJournalFolderRequestModel = {
      name: folderName,
      gameId: this.gameId,
      parentFolderId: parentFolderId ? parentFolderId : Guid.getEmptyGuid()
    };

    this.journalService.addFolderToGame(folderRequest);
  }
}
