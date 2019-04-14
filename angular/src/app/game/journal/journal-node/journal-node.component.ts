import { Component, OnInit, Input, ViewChild, ElementRef, ViewChildren, Output, EventEmitter } from '@angular/core';
import { MatMenuTrigger } from '@angular/material';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';

@Component({
  selector: 'trpg-journal-node',
  templateUrl: './journal-node.component.html',
  styleUrls: ['./journal-node.component.scss']
})
export class JournalNodeComponent {
  @Input() journalNode: JournalItem;
  @Output() addJournalFolder = new EventEmitter<JournalFolder>();
  @Output() clickFolder = new EventEmitter<JournalItem>();

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

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
    this.addJournalFolder.emit(this.journalNode as JournalFolder);
  }

  clickedFolder() {
    this.clickFolder.emit(this.journalNode);
  }
}
