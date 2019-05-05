import { Component, OnInit } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalService } from './journal.service';
import { MatDialog } from '@angular/material';
import { Guid } from 'src/app/utilities/guid.util';
import { AddedJournalItemModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { TreeTraversal } from 'src/app/utilities/tree-traversal.util';
import { ActiveGameService } from '../services/active-game.service';
import { Player } from 'src/app/models/game/player.model';
import { ParentDialogComponent } from './parent-dialog/parent-dialog.component';
import { ParentDialogModel } from './parent-dialog/parent-dialog.model';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';

@Component({
  selector: 'trpg-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent implements OnInit {
  isOwner: boolean;
  players: Player[] = [];

  journalItems: JournalItem[] = [];

  subIcons = ['create_new_folder', 'person_add', 'note_add'];

  treeControl = new NestedTreeControl<JournalItem>((node: JournalFolder) => node.journalItems);
  dataSource = new MatTreeNestedDataSource<JournalItem>();

  hasChild = (_: number, node: JournalFolder) => !!node.journalItems;

  constructor(private journalService: JournalService, private activeGameService: ActiveGameService, public dialog: MatDialog) {
    this.dataSource.data = this.journalItems;
  }

  ngOnInit() {
    this.journalService.setup();
    this.journalService.journalItemAdded.subscribe((model: AddedJournalItemModel) => {
      this.addJournalItem(model);
    });
    this.journalService.getAllJournalItems().subscribe(data => {
      this.journalItems = data;
      this.refreshDataSource();
    });

    this.activeGameService.isOwnerObservable.subscribe((isOwner) => {
      this.isOwner = isOwner;
    });

    this.activeGameService.playersObservable.subscribe((players) => {
      this.players = players;
    });
  }

  subIconClicked(icon: string) {
    switch (icon) {
      case this.subIcons[0]:
        this.openDialog(new JournalFolder());
        break;
      case this.subIcons[2]:
        this.openDialog(new JournalHandout());
        break;
    }
  }

  addJournalItem(model: AddedJournalItemModel): void {
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
    this.openDialog(new JournalFolder(), parentFolder.id);
  }

  clickFolder(node: JournalItem) {
    this.treeControl.toggle(node);
  }

  private findChild(model: AddedJournalItemModel) {
    return TreeTraversal.findChild(model.parentId, this.journalItems,
      (item: JournalItem) => (item as JournalFolder).journalItems, (id: string, item: JournalItem) => id === item.id) as JournalFolder;
  }

  private refreshDataSource() {
    this.dataSource.data = null;
    this.dataSource.data = this.journalItems;
  }

  private openDialog(journalItem: JournalItem, parentFolderId: string = Guid.getEmptyGuid()) {
    const data: ParentDialogModel = {
      players: this.players,
      isOwner: this.isOwner,
      parentFolderId: parentFolderId,
      data: journalItem
    };
    this.dialog.open(ParentDialogComponent, {
      width: 'auto',
      data: data,
      hasBackdrop: false
    });
  }
}
