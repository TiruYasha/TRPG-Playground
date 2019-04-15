import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameService } from '../shared/game.service';
import { ActiveGameService } from '../shared/services/active-game.service';
import { Player } from '../models/game/player.model';

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
        this.isOwner = isOwner;
      });

    this.gameService.getPlayers()
      .subscribe((players) => {
        console.log(players);
        this.players = players;
      });
  }
}
