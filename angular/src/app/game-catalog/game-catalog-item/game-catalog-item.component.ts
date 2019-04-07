import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { GameCatalogItem } from 'src/app/models/game-catalog/game-catalog-item.model';

@Component({
  selector: 'trpg-game-catalog-item',
  templateUrl: './game-catalog-item.component.html',
  styleUrls: ['./game-catalog-item.component.scss']
})
export class GameCatalogItemComponent implements OnInit {
  @Input() game: GameCatalogItem;

  constructor() { }

  ngOnInit() {
  }
}
