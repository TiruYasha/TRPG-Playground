import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from '../shared/game.service';

@Component({
  selector: 'trpg-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit {
  isOwner = false;
  gameId = '';

  constructor(private _activatedRoute: ActivatedRoute, private gameService: GameService) {
  }

  ngOnInit() {
    const id = this._activatedRoute.snapshot.params['id'];

    this.gameId = id;

    this.gameService.joinGame(id)
      .subscribe((isOwner) => {
        this.isOwner = isOwner;
      });
  }
}
