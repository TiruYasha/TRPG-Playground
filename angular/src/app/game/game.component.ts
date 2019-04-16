import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameService } from '../shared/game.service';
import { Player } from '../models/game/player.model';
import { ActiveGameService } from './services/active-game.service';

@Component({
  selector: 'trpg-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {
  players: Player[] = [];
  isOwner = false;
  gameId = '';

  constructor(private _activatedRoute: ActivatedRoute, private gameService: GameService, private activeGameService: ActiveGameService) {
  }

  ngOnInit() {
    const id = this._activatedRoute.snapshot.params['id'];

    this.gameId = id;
    this.activeGameService.activeGameId = id;

    this.gameService.joinGame()
      .subscribe((isOwner) => {
        this.activeGameService.updateIsOwner(isOwner);
      });

    this.gameService.getPlayers()
      .subscribe((players) => {
        this.activeGameService.updatePlayers(players);
      });
  }
}
