import { Component, OnInit } from '@angular/core';
import { GameCatalogService } from './services/game-catalog.service';
import { Observable } from 'rxjs';
import { GameCatalogItem } from '../shared/models/game-catalog/game-catalog-item.model';
import { Router } from '@angular/router';

@Component({
  selector: 'trpg-game-catalog',
  templateUrl: './game-catalog.component.html',
  styleUrls: ['./game-catalog.component.scss']
})
export class GameCatalogComponent implements OnInit {
  games: Observable<GameCatalogItem[]>;

  constructor(private gameCatalogService: GameCatalogService) {

  }

  ngOnInit() {
    this.games = this.gameCatalogService.getGames();
  }
}
