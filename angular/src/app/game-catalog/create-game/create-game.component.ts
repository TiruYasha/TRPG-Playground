import { Component, OnInit } from '@angular/core';
import { GameCatalogService } from '../services/game-catalog.service';
import { CreateGameModel } from 'src/app/models/game-catalog/creategame.model';

@Component({
  selector: 'trpg-create-game',
  templateUrl: './create-game.component.html',
  styleUrls: ['./create-game.component.scss']
})
export class CreateGameComponent implements OnInit {

  gameName = '';

  constructor(private gameCatalogService: GameCatalogService) { }

  ngOnInit() {
  }

  createGame(): void {
    const game = new CreateGameModel(this.gameName);
    console.log('Create a game', this.gameName);
    this.gameCatalogService.createGame(game)
      .subscribe((gameId: string) => {

        console.log('created the game');
      });
  }
}
