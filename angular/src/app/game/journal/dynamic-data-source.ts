import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable, merge } from 'rxjs';

import { DynamicFlatNode } from 'src/app/models/journal/dynamic-flat-node';

import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';

import { FlatTreeControl } from '@angular/cdk/tree';

import { JournalService } from './journal.service';

import { CollectionViewer, SelectionChange } from '@angular/cdk/collections';

import { map } from 'rxjs/operators';

import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';

@Injectable()
export class JournalDynamicDataSource {

  dataChange = new BehaviorSubject<DynamicFlatNode<JournalItem>[]>([]);

  get data(): DynamicFlatNode<JournalItem>[] { return this.dataChange.value; }
  set data(value: DynamicFlatNode<JournalItem>[]) {
    this.treeControl.dataNodes = value;
    this.dataChange.next(value);
  }

  constructor(private treeControl: FlatTreeControl<DynamicFlatNode<JournalItem>>,
    private journalService: JournalService) { }

  connect(collectionViewer: CollectionViewer): Observable<DynamicFlatNode<JournalItem>[]> {
    this.treeControl.expansionModel.changed.subscribe(change => {
      if ((change as SelectionChange<DynamicFlatNode<JournalItem>>).added ||
        (change as SelectionChange<DynamicFlatNode<JournalItem>>).removed) {
        this.handleTreeControl(change as SelectionChange<DynamicFlatNode<JournalItem>>);
      }
    });

    return merge(collectionViewer.viewChange, this.dataChange).pipe(map(() => this.data));
  }

  /** Handle expand/collapse behaviors */
  handleTreeControl(change: SelectionChange<DynamicFlatNode<JournalItem>>) {
    if (change.added) {
      change.added.forEach(node => this.toggleNode(node, true));
    }
    if (change.removed) {
      change.removed.slice().reverse().forEach(node => this.toggleNode(node, false));
    }
  }

  /**
   * Toggle the node, remove from display list
   */
  toggleNode(node: DynamicFlatNode<JournalItem>, expand: boolean) {
    const index = this.data.indexOf(node);
    node.isLoading = true;
    this.journalService.getJournalItemsByParentFolderId(node.item.id)
      .subscribe((data: JournalItem[]) => {
        if (!data || index < 0) {
          return;
        }

        if (expand) {
          const nodes = data.map(item => new DynamicFlatNode<JournalItem>(item, node.level + 1, item.type === JournalItemType.Folder));
          this.data.splice(index + 1, 0, ...nodes);
        } else {
          let count = 0;
          for (let i = index + 1; i < this.data.length
            && this.data[i].level > node.level; i++ , count++) { }
          this.data.splice(index + 1, count);
        }

        this.dataChange.next(this.data);
        node.isLoading = false;
      });
  }
}
