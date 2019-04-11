import { Component, OnInit, Input, ViewChild, ElementRef, ViewChildren } from '@angular/core';
import { JournalNode } from 'src/app/models/journal/journal-node.model';
import { MatMenuTrigger, MatMenu } from '@angular/material';

@Component({
  selector: 'trpg-journal-node',
  templateUrl: './journal-node.component.html',
  styleUrls: ['./journal-node.component.scss']
})
export class JournalNodeComponent implements OnInit {
  @Input() journalNode: JournalNode;

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  constructor() { }

  ngOnInit() {

  }

  triggerMenu(event: MouseEvent) {
    event.preventDefault();
    console.log(event);
    console.log(this.button);
    const button = this.button.nativeElement as HTMLDivElement
    button.style.position = 'absolute';
    button.style.left = `${event.offsetX}px`;
    button.style.top = `${event.pageY - 48}px`;

    this.trigger.openMenu();
  }
}
