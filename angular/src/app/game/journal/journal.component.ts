import { Component, OnInit } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { JournalService } from '../services/journal.service';
import { MatDialog } from '@angular/material';
import { AddedJournalItemModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { GameHubService } from '../services/game-hub.service';
import { Player } from 'src/app/models/game/player.model';
import { ParentDialogComponent } from './parent-dialog/parent-dialog.component';
import { ParentDialogModel } from './parent-dialog/parent-dialog.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';
import { JournalNodeContextMenuAddClick } from './journal-node/journal-node-context-menu-click.model';
import { DynamicFlatNode } from 'src/app/models/journal/dynamic-flat-node';
import { JournalDynamicDataSource } from './dynamic-data-source';
import { environment } from 'src/environments/environment';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { takeUntil } from 'rxjs/operators';
import { DialogState } from '../../models/dialog-state.enum';
import { JournalTreeItem } from 'src/app/models/journal/journal-tree-item.model';
import { GameStateService } from '../services/game-state.service';

@Component({
  selector: 'trpg-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent extends DestroySubscription implements OnInit {
  isOwner: boolean;
  players: Player[] = [];

  journalItemsNodes: DynamicFlatNode<JournalTreeItem>[] = [];

  subIcons = ['create_new_folder', 'person_add', 'note_add'];

  treeControl: FlatTreeControl<DynamicFlatNode<JournalTreeItem>>;
  dataSource: JournalDynamicDataSource;

  getLevel = (node: DynamicFlatNode<JournalTreeItem>) => node.level;
  IsExpandable = (node: DynamicFlatNode<JournalTreeItem>) => node.item.type === JournalItemType.Folder;

  hasChild = (_: number, node: DynamicFlatNode<JournalTreeItem>) => this.IsExpandable(node);

  constructor(private journalService: JournalService, private gameState: GameStateService, public dialog: MatDialog) {
    super();
    this.treeControl = new FlatTreeControl<DynamicFlatNode<JournalTreeItem>>(this.getLevel, this.IsExpandable);
    this.dataSource = new JournalDynamicDataSource(this.treeControl, journalService);
    this.dataSource.data = this.journalItemsNodes;
  }

  ngOnInit() {
    this.journalService.setup();
    this.journalService.journalItemAdded
      .pipe(takeUntil(this.destroy))
      .subscribe((model: AddedJournalItemModel) => {
        this.addedJournalItem(model);
      });
    this.journalService.getRootJournalItems()
      .pipe(takeUntil(this.destroy))
      .subscribe(data => {
        const nodes = data.map(item => new DynamicFlatNode<JournalTreeItem>(item, 0));
        this.dataSource.data = nodes;
      });

    this.gameState.isOwnerObservable
      .pipe(takeUntil(this.destroy))
      .subscribe((isOwner) => {
        this.isOwner = isOwner;
      });

    this.gameState.playersObservable
      .pipe(takeUntil(this.destroy))
      .subscribe((players) => {
        this.players = players;
      });

    this.journalService.journalItemImageUploaded
      .pipe(takeUntil(this.destroy))
      .subscribe((image) => {
        this.refreshDataSource();
      });

    this.journalService.journalItemUpdated
      .pipe(takeUntil(this.destroy))
      .subscribe(journalTreeItem => this.updatedJournalItem(journalTreeItem));

    this.journalService.journalItemDeleted
      .pipe(takeUntil(this.destroy))
      .subscribe(journalItemId => this.removeItemFromTree(journalItemId));
  }

  subIconClicked(icon: string) {
    switch (icon) {
      case this.subIcons[0]:
        this.openDialog(JournalItemType.Folder, DialogState.New);
        break;
      case this.subIcons[1]:
        this.openDialog(JournalItemType.CharacterSheet, DialogState.New);
        break;
      case this.subIcons[2]:
        this.openDialog(JournalItemType.Handout, DialogState.New);
        break;
    }
  }

  addedJournalItem(model: AddedJournalItemModel): void {
    const journalItem = new JournalTreeItem();
    journalItem.type = model.type;
    journalItem.name = model.name;
    journalItem.id = model.id;
    journalItem.imageId = model.imageId;
    journalItem.parentFolderId = model.parentFolderId;

    this.addJournalItemToTree(journalItem);
  }

  updatedJournalItem(journalTreeItem: JournalTreeItem) {
    const journalTreeItemToUpdate = this.dataSource.data.filter(d => d.item.id === journalTreeItem.id)[0];

    if (journalTreeItemToUpdate) {
      journalTreeItemToUpdate.item.name = journalTreeItem.name;
      journalTreeItemToUpdate.item.canEdit = journalTreeItem.canEdit;

      this.refreshDataSource();
    } else {
      this.addJournalItemToTree(journalTreeItem);
    }
  }

  removeItemFromTree(journalItemId: string): void {
    const node = this.dataSource.data.filter(j => j.item.id === journalItemId)[0];
    if (node.item.type === JournalItemType.Folder) {
      const parentIds: string[] = [];
      parentIds.push(node.item.id);

      while (parentIds.length > 0) {
        const parentId = parentIds.pop();
        const childNodes = this.dataSource.data.filter(j => j.item.parentFolderId === parentId);
        this.dataSource.data = this.dataSource.data.filter(j => j.item.id !== node.item.id);
        childNodes.forEach(childNode => {
          if (childNode.item.type === JournalItemType.Folder) {
            parentIds.push(childNode.item.id);
          }

          this.dataSource.data = this.dataSource.data.filter(j => j.item.id !== childNode.item.id);
        });
      }
    } else {
      this.dataSource.data = this.dataSource.data.filter(j => j.item.id !== journalItemId);
    }
    this.refreshDataSource();
  }

  addJournalItemToParent(click: JournalNodeContextMenuAddClick) {
    this.openDialog(click.type, DialogState.New, null, click.id);
  }

  clickFolder(node: DynamicFlatNode<JournalTreeItem>) {
    this.treeControl.toggle(node);
  }

  clickItem(node: DynamicFlatNode<JournalTreeItem>) {
    this.openDialog(node.item.type, DialogState.View, node.item.id);
  }

  getThumbnailLink(journalItemId: string) {
    return `${environment.apiUrl}/journal/${journalItemId}/image`;
  }

  editItem(journalItem: JournalTreeItem) {
    this.openDialog(journalItem.type, DialogState.Edit, journalItem.id);
  }

  deleteItem(journalItem: JournalTreeItem) {
    this.journalService.deleteJournalItem(journalItem.id)
      .pipe(takeUntil(this.destroy))
      .subscribe();
  }

  private addJournalItemToTree(journalItem: JournalTreeItem) {
    let node = new DynamicFlatNode<JournalTreeItem>(journalItem, 0);
    if (journalItem.parentFolderId) {
      const parentFolder = this.dataSource.data.filter(d => d.item.id === journalItem.parentFolderId)[0];
      if (parentFolder && this.treeControl.isExpanded(parentFolder)) {
        node = new DynamicFlatNode<JournalTreeItem>(journalItem, parentFolder.level + 1);
        const index = this.dataSource.data.indexOf(parentFolder);
        this.dataSource.data.splice(index + 1, 0, node);
      }
    } else {
      this.dataSource.data.push(node);
    }
    this.refreshDataSource();
  }

  private openDialog(journalItemType: JournalItemType, state: DialogState, journalItemId: string = null, parentFolderId: string = null) {
    const treeItem = this.dataSource.data.filter(d => d.item.id === journalItemId)[0];

    const data: ParentDialogModel = {
      players: this.players,
      isOwner: this.isOwner,
      parentFolderId: parentFolderId,
      journalItemId: journalItemId,
      state: state,
      journalItemType: journalItemType,
      canEdit: state === DialogState.New ? true :  treeItem.item.canEdit
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
