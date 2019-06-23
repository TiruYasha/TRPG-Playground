import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { PlayMap } from 'src/app/models/map/map.model';

@Component({
  selector: 'trpg-map-list',
  templateUrl: './map-list.component.html',
  styleUrls: ['./map-list.component.scss']
})
export class MapListComponent implements OnInit {

  @Output() addMap = new EventEmitter();

  @Input() maps: PlayMap[] = [];

  constructor() { }

  ngOnInit() {

  }
}
