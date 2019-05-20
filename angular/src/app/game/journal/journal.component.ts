import { Component, OnInit } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalService } from './journal.service';
import { MatDialog } from '@angular/material';
import { AddedJournalItemModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { ActiveGameService } from '../services/active-game.service';
import { Player } from 'src/app/models/game/player.model';
import { ParentDialogComponent } from './parent-dialog/parent-dialog.component';
import { ParentDialogModel } from './parent-dialog/parent-dialog.model';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';
import { JournalNodeContextMenuClick } from './journal-node/journal-node-context-menu-click.model';
import { DynamicFlatNode } from 'src/app/models/journal/dynamic-flat-node';
import { JournalDynamicDataSource } from './dynamic-data-source';

@Component({
  selector: 'trpg-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent implements OnInit {
  isOwner: boolean;
  players: Player[] = [];

  journalItemsNodes: DynamicFlatNode<JournalItem>[] = [];

  subIcons = ['create_new_folder', 'person_add', 'note_add'];

  treeControl: FlatTreeControl<DynamicFlatNode<JournalItem>>;
  dataSource: JournalDynamicDataSource;

  getLevel = (node: DynamicFlatNode<JournalItem>) => node.level;
  IsExpandable = (node: DynamicFlatNode<JournalItem>) => node.item.type === JournalItemType.Folder;

  hasChild = (_: number, node: DynamicFlatNode<JournalItem>) => this.IsExpandable(node);

  constructor(private journalService: JournalService, private activeGameService: ActiveGameService, public dialog: MatDialog) {
    this.treeControl = new FlatTreeControl<DynamicFlatNode<JournalItem>>(this.getLevel, this.IsExpandable);
    this.dataSource = new JournalDynamicDataSource(this.treeControl, journalService);
    this.dataSource.data = this.journalItemsNodes;
  }

  ngOnInit() {
    this.journalService.setup();
    this.journalService.journalItemAdded.subscribe((model: AddedJournalItemModel) => {
      this.addJournalItem(model);
    });
    this.journalService.getRootJournalItems().subscribe(data => {
      const nodes = data.map(item => new DynamicFlatNode<JournalItem>(item, 0));
      this.dataSource.data = nodes;
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
    const journalItem = new JournalItem(model.type);
    journalItem.name = model.name;
    journalItem.id = model.id;
    journalItem.imageId = model.imageId;

    let node = new DynamicFlatNode<JournalItem>(journalItem, 0);

    if (model.parentFolderId) {
      const parentFolder = this.dataSource.data.filter(d => d.item.id === model.parentFolderId)[0];

      if (parentFolder && this.treeControl.isExpanded(parentFolder)) {
        node = new DynamicFlatNode<JournalItem>(journalItem, parentFolder.level + 1);
        const index = this.dataSource.data.indexOf(parentFolder);
        this.dataSource.data.splice(index + 1, 0, node);
      }
    } else {
      this.dataSource.data.push(node);
    }
    this.refreshDataSource();
  }

  addJournalItemToParent(click: JournalNodeContextMenuClick) {
    this.openDialog(click.item, click.id);
  }

  clickFolder(node: DynamicFlatNode<JournalItem>) {
    this.treeControl.toggle(node);
  }

  private openDialog(journalItem: JournalItem, parentFolderId: string = null) {
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

  private refreshDataSource() {
    const source = this.dataSource.data;
    this.dataSource.data = null;
    this.dataSource.data = source;
  }
}
