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
  @Output() deleteItem = new EventEmitter<JournalTreeItem>();

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

    this.trigger.menu.hasBackdrop = true;
    this.trigger.openMenu();
    document.getElementsByClassName('cdk-overlay-backdrop')[0].addEventListener('contextmenu', (offEvent: MouseEvent) => {
      //Temporary right click fix
      this.trigger.closeMenu();
      offEvent.preventDefault();
    });
  }

  editItemClick(journalItem: JournalTreeItem) {
    this.editItem.next(journalItem);
  }

  deleteItemClick(journalItem: JournalTreeItem) {
    this.deleteItem.next(journalItem);
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
