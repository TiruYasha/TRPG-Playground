import { Component, Input, ViewChild, ElementRef, Output, EventEmitter, OnInit } from '@angular/core';
import { MatMenuTrigger } from '@angular/material';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { JournalNodeContextMenuClick } from './journal-node-context-menu-click.model';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';

@Component({
  selector: 'trpg-journal-node',
  templateUrl: './journal-node.component.html',
  styleUrls: ['./journal-node.component.scss']
})
export class JournalNodeComponent {
  @Input() journalItem: JournalItem;
  @Input() isOwner: boolean;
  @Output() addJournalItem = new EventEmitter<JournalNodeContextMenuClick>();
  @Output() clickFolder = new EventEmitter<JournalItem>();

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  type = JournalItemType;

  constructor() { }

  triggerMenu(event: MouseEvent) {
    event.preventDefault();
    const button = this.button.nativeElement as HTMLDivElement;
    button.style.position = 'absolute';
    button.style.left = `${event.offsetX}px`;
    button.style.top = `${event.pageY - 48}px`;

    this.trigger.openMenu();
  }

  addFolder() {
    this.sendContextMenuClick(new JournalFolder());
  }

  addHandout() {
    this.sendContextMenuClick(new JournalHandout());
  }

  clickedFolder() {
    this.clickFolder.emit(this.journalItem);
  }

  private sendContextMenuClick(item: JournalItem){
    const click: JournalNodeContextMenuClick = {
      id: this.journalItem.id,
      item: item
    };

    this.addJournalItem.emit(click);
  }
}
