import { Component, OnInit } from '@angular/core';
import { NestedTreeControl } from '@angular/cdk/tree';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { JournalItem } from './models/journal-item.model';
import { Journal } from './models/journal.model';

@Component({
  selector: 'trpg-journal',
  templateUrl: './journal.component.html',
  styleUrls: ['./journal.component.scss']
})
export class JournalComponent implements OnInit {
  nestedDatasource: JournalItem[] = [];

  nestedTreeControl: NestedTreeControl<JournalItem>;
  nestedDataSource: MatTreeNestedDataSource<JournalItem>;

  constructor() {
    const journalItem: JournalItem = {
      id: '',
      type: 2,
      journalItems: []
    };
    const journalItems = [journalItem];
    this.nestedDatasource = journalItems;
    this.nestedTreeControl = new NestedTreeControl<JournalItem>(this._getJournalItems);
  }

  ngOnInit() {
  }

  hasNestedChild = (_: number, nodeData: JournalItem) => !nodeData.type;

  private _getJournalItems = (node: JournalItem) => node.journalItems;

}
