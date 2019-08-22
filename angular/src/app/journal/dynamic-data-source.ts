import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, merge } from 'rxjs';
import { FlatTreeControl } from '@angular/cdk/tree';
import { CollectionViewer, SelectionChange } from '@angular/cdk/collections';
import { map } from 'rxjs/operators';
import { DynamicFlatNode } from 'src/app/shared/models/journal/dynamic-flat-node';
import { JournalTreeItem } from 'src/app/shared/models/journal/journal-tree-item.model';
import { JournalItemType } from 'src/app/shared/models/journal/journalitems/journal-item-type.enum';
import { JournalService } from '../shared/services/journal.service';

@Injectable()
export class JournalDynamicDataSource {

  dataChange = new BehaviorSubject<DynamicFlatNode<JournalTreeItem>[]>([]);

  get data(): DynamicFlatNode<JournalTreeItem>[] { return this.dataChange.value; }
  set data(value: DynamicFlatNode<JournalTreeItem>[]) {
    this.treeControl.dataNodes = value;
    this.dataChange.next(value);
  }

  constructor(private treeControl: FlatTreeControl<DynamicFlatNode<JournalTreeItem>>,
    private journalService: JournalService) { }

  connect(collectionViewer: CollectionViewer): Observable<DynamicFlatNode<JournalTreeItem>[]> {
    this.treeControl.expansionModel.changed.subscribe(change => {
      if ((change as SelectionChange<DynamicFlatNode<JournalTreeItem>>).added ||
        (change as SelectionChange<DynamicFlatNode<JournalTreeItem>>).removed) {
        this.handleTreeControl(change as SelectionChange<DynamicFlatNode<JournalTreeItem>>);
      }
    });

    return merge(collectionViewer.viewChange, this.dataChange).pipe(map(() => this.data));
  }

  /** Handle expand/collapse behaviors */
  handleTreeControl(change: SelectionChange<DynamicFlatNode<JournalTreeItem>>) {
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
  toggleNode(node: DynamicFlatNode<JournalTreeItem>, expand: boolean) {
    const index = this.data.indexOf(node);
    node.isLoading = true;
    this.journalService.getJournalItemsByParentFolderId(node.item.id)
      .subscribe((data: JournalTreeItem[]) => {
        if (!data || index < 0) {
          return;
        }

        if (expand) {
          const nodes = data.map(item => new DynamicFlatNode<JournalTreeItem>(item, node.level + 1, item.type === JournalItemType.Folder));
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
