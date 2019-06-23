import { Component, OnInit, Output, Input, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { PlayMap } from 'src/app/models/map/map.model';
import { MatMenuTrigger } from '@angular/material';

@Component({
  selector: 'trpg-map-list',
  templateUrl: './map-list.component.html',
  styleUrls: ['./map-list.component.scss']
})
export class MapListComponent implements OnInit {
  selectedMap: PlayMap;

  @Output() addMap = new EventEmitter();
  @Output() selectMap = new EventEmitter<PlayMap>();

  @Input() maps: PlayMap[] = [];

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  constructor() { }

  ngOnInit() {

  }

  onSelectedMap(map: PlayMap) {
    this.selectMap.emit(map);

    this.selectedMap = map;
  }

  triggerMenu(event: MouseEvent) {
    event.preventDefault();
    const button = this.button.nativeElement as HTMLDivElement;
    button.style.position = 'absolute';
    button.style.left = `${event.offsetX - 64}px`;
    button.style.top = `${event.pageY - 80}px`;

    this.trigger.menu.hasBackdrop = true;
    this.trigger.openMenu();
    document.getElementsByClassName('cdk-overlay-backdrop')[0].addEventListener('contextmenu', (offEvent: MouseEvent) => {
      offEvent.preventDefault();
      //Temporary right click fix
      this.trigger.closeMenu();
    });
  }

  sendEditClick() {
    //this.addJournalItem.emit(click);
  }
}
