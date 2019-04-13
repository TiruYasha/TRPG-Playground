import { Component, OnInit, Input, ViewChild, ElementRef, ViewChildren, Output, EventEmitter } from '@angular/core';
import { JournalNode } from 'src/app/models/journal/journal-node.model';
import { MatMenuTrigger } from '@angular/material';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';

@Component({
  selector: 'trpg-journal-node',
  templateUrl: './journal-node.component.html',
  styleUrls: ['./journal-node.component.scss']
})
export class JournalNodeComponent {
  @Input() journalNode: JournalNode;
  @Output() addJournalFolder = new EventEmitter<JournalFolder>();

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
    this.addJournalFolder.emit(this.journalNode.item as JournalFolder);
  }
}
