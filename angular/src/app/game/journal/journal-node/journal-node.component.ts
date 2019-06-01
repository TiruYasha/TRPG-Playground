import { Component, Input, ViewChild, ElementRef, Output, EventEmitter, OnInit } from '@angular/core';
import { MatMenuTrigger } from '@angular/material';
import { JournalNodeContextMenuAddClick } from './journal-node-context-menu-click.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';
import { JournalTreeItem } from 'src/app/models/journal/journal-tree-item.model';

@Component({
  selector: 'trpg-journal-node',
  templateUrl: './journal-node.component.html',
  styleUrls: ['./journal-node.component.scss']
})
export class JournalNodeComponent {
  @Input() journalItem: JournalTreeItem;
  @Input() isOwner: boolean;
  @Output() addJournalItem = new EventEmitter<JournalNodeContextMenuAddClick>();
  @Output() clickItem = new EventEmitter<JournalTreeItem>();
  @Output() editItem = new EventEmitter<JournalTreeItem>();

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  type = JournalItemType;

  constructor() {

  }

  triggerMenu(event: MouseEvent) {
    event.preventDefault();
    const button = this.button.nativeElement as HTMLDivElement;
    button.style.position = 'absolute';
    button.style.left = `${event.offsetX}px`;
    button.style.top = `${event.pageY - 48}px`;

    this.trigger.openMenu();
  }

  editItemClick(journalItem: JournalTreeItem) {
    this.editItem.next(journalItem);
  }

  clickedFolder() {
    this.clickItem.emit(this.journalItem);
  }

  sendContextMenuClick(type: JournalItemType) {
    const click: JournalNodeContextMenuAddClick = {
      id: this.journalItem.id,
      type
    };

    this.addJournalItem.emit(click);
  }
}
