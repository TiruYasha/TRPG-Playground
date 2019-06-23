import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { PlayMap } from 'src/app/models/map/map.model';

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

  constructor() { }

  ngOnInit() {

  }

  onSelectedMap(map: PlayMap) {
    this.selectMap.emit(map);

    this.selectedMap = map;
  }
}
