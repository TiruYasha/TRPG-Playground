import { Component, OnInit, Input } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalService } from './journal.service';
import { CreateFolderDialogComponent } from './create-folder-dialog/create-folder-dialog.component';
import { MatDialog } from '@angular/material';
import { AddJournalFolderRequestModel } from 'src/app/models/journal/requests/AddJournalFolderRequest.model';
import { Guid } from 'src/app/utilities/guid.util';
import { AddedJournalFolderModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { JournalNode } from '../../models/journal/journal-node.model';

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

  treeControl = new FlatTreeControl<JournalNode>(
    node => node.level, node => node.expandable
  );

  treeFlattener: MatTreeFlattener<JournalItem, JournalNode>;

  dataSource: MatTreeFlatDataSource<JournalItem, JournalNode>;

  hasChild = (_: number, node: JournalNode) => node.expandable;

  constructor(private journalService: JournalService, public dialog: MatDialog) {
    this.treeFlattener = new MatTreeFlattener(
      this.transformer,
      node => node.level,
      node => node.expandable,
      node => (<JournalFolder>node).journalItems
    );

    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener)
    const item = new JournalFolder();
    item.name = 'testing';
    this.journalItems.push(item);

    this.dataSource.data = this.journalItems;
  }

  ngOnInit() {
    this.journalService.setup(this.gameId);

    this.journalService.AddedJournalFolder.subscribe((model: AddedJournalFolderModel) => this.addFolderToJournalItems(model));
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

    this.journalItems.push(folder);
    this.dataSource.data = this.journalItems;
  }

  private openCreateNewFolderDialog() {
    const dialogRef = this.dialog.open(CreateFolderDialogComponent, {
      width: '250px',
      data: { name: '' }
    });

    dialogRef.afterClosed().subscribe((folderName) => this.createNewFolder(folderName));
  }

  private createNewFolder(folderName) {
    if (folderName === void 0) {
      return;
    }

    const folderRequest: AddJournalFolderRequestModel = {
      name: folderName,
      gameId: this.gameId,
      parentFolder: Guid.getEmptyGuid()
    };

    this.journalService.addFolderToGame(folderRequest);
  }

  private transformer = (node: JournalItem, level: number): JournalNode => {
    const journalFolder = node as JournalFolder;

    return {
      expandable: !!journalFolder,
      item: node,
      level: level
    };
  }
}
