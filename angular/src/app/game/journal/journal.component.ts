import { Component, OnInit, Input } from '@angular/core';
import { FlatTreeControl, NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener, MatTreeNestedDataSource } from '@angular/material/tree';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalService } from './journal.service';
import { CreateFolderDialogComponent } from './create-folder-dialog/create-folder-dialog.component';
import { MatDialog } from '@angular/material';
import { AddJournalFolderRequestModel } from 'src/app/models/journal/requests/AddJournalFolderRequest.model';
import { Guid } from 'src/app/utilities/guid.util';
import { AddedJournalFolderModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';

type getChildren = (item: any) => any[];
type compare = (id: any, item: any) => boolean;

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
    const journalItem = new JournalFolder();
    journalItem.id = 'dsfsd';
    journalItem.name = 'dsfdasf';
    const nested = new JournalFolder();
    nested.id = 'sadasd';
    nested.name = 'dsfasdfasd';
    journalItem.journalItems.push(nested);

    this.journalItems.push(journalItem);
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
  }

  addJournalFolder(parentFolder: JournalFolder) {
    const item = new JournalFolder();
    item.name = 'testing';

    const child = this.findChild(
      parentFolder.id,
      this.journalItems,
      (item: JournalItem) => (item as JournalFolder).journalItems,
      (id: string, item: JournalItem) => id === item.id) as JournalFolder;

    child.journalItems.push(item);

    this.refreshDataSource();
  }

  private refreshDataSource() {
    this.dataSource.data = null;
    this.dataSource.data = this.journalItems;
  }

  private findChild<ID, ITEM>(id: ID, items: ITEM[], getChildren: getChildren, compare: compare): ITEM {
    for (let item of items) {
      compare(id, item);
      if (compare(id, item)) {
        return item;
      }

      const child = this.findChildImpl(id, item, getChildren, compare);

      if (child) {
        return child;
      }
    }

    return null;
  }

  private findChildImpl<ID, ITEM>(id: ID, root: ITEM, getChildren: getChildren, compare: compare): ITEM {
    const stack: ITEM[] = []
    let node: ITEM, ii;
    stack.push(root);

    while (stack.length > 0) {
      node = stack.pop();
      const children = getChildren(node);
      if (compare(id, node)) {
        return node;
      } else if (children && children.length) {
        for (ii = 0; ii < children.length; ii += 1) {
          stack.push(children[ii]);
        }
      }
    }

    return null;
  }

  // private findChildImpl(root: JournalFolder, id: string) {
  //   const stack: JournalFolder[] = []
  //   let node: JournalFolder, ii;
  //   stack.push(root);

  //   while (stack.length > 0) {
  //     node = stack.pop();
  //     if (node.id === id) {
  //       return node;
  //     } else if (node.journalItems && node.journalItems.length) {
  //       for (ii = 0; ii < node.journalItems.length; ii += 1) {
  //         stack.push(node.journalItems[ii] as JournalFolder);
  //       }
  //     }
  //   }

  //   return null;
  // }

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
}
