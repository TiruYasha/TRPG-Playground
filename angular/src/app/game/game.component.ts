import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActiveGameService } from './services/active-game.service';
import { GameService } from '../shared/game.service';

@Component({
  selector: 'trpg-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {

  constructor(private _activatedRoute: ActivatedRoute, private _activeGameService: ActiveGameService, private gameService: GameService) {
   }

  ngOnInit() {
    const id = this._activatedRoute.snapshot.params['id'];

    this._activeGameService.gameId = id;

    this.gameService.joinGame(id)
      .subscribe(() => {
        console.log("succesfully joined the game");
      });
  }
}
