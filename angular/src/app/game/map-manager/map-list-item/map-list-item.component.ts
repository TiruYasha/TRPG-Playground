import { Component, OnInit, Output, EventEmitter, Input, ViewChild, ElementRef } from '@angular/core';
import { PlayMap } from 'src/app/models/map/map.model';
import { MatMenuTrigger } from '@angular/material';

@Component({
  selector: 'trpg-map-list-item',
  templateUrl: './map-list-item.component.html',
  styleUrls: ['./map-list-item.component.scss']
})
export class MapListItemComponent implements OnInit {
  @Input() map: PlayMap;
  @Input() selected: boolean;
  @Input() visible = false;

  @Output() selectMap = new EventEmitter<PlayMap>();
  @Output() editMap = new EventEmitter<PlayMap>();
  @Output() deleteMap = new EventEmitter<PlayMap>();
  @Output() makeVisible = new EventEmitter();

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger;
  @ViewChild('menuButton') button: ElementRef;

  constructor() { }

  ngOnInit() {
  }

  onSelectedMap(map: PlayMap) {
    this.selectMap.emit(map);
  }

  toggleVisibillity() {
    this.makeVisible.emit();
  }

  triggerMenu(event: MouseEvent) {
    event.preventDefault();
    const button = this.button.nativeElement as HTMLDivElement;
    button.style.position = 'absolute';
    button.style.left = `${event.pageX - 64}px`;

    this.trigger.menu.hasBackdrop = true;
    this.trigger.openMenu();
    document.getElementsByClassName('cdk-overlay-backdrop')[0].addEventListener('contextmenu', (offEvent: MouseEvent) => {
      offEvent.preventDefault();
      //Temporary right click fix
      this.trigger.closeMenu();
    });
  }
}
