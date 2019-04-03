import { Component, OnInit } from '@angular/core';
import { GameCatalogService } from '../services/game-catalog.service';
import { CreateGameModel } from 'src/app/models/game/creategame.model';

@Component({
  selector: 'trpg-create-game',
  templateUrl: './create-game.component.html',
  styleUrls: ['./create-game.component.css']
})
export class CreateGameComponent implements OnInit {

  gameName: string = '';

  constructor(private gameCatalogService: GameCatalogService) { }

  ngOnInit() {
  }

  createGame(): void{
    const game = new CreateGameModel(this.gameName);
    console.log('Create a game', this.gameName);
    this.gameCatalogService.createGame(game)
      .subscribe(() => {
        // Return gameId, so also change backend
        console.log('created the game');
      });
  }
}
