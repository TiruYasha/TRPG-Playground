import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameService } from '../shared/game.service';
import { Player } from '../models/game/player.model';
import { ActiveGameService } from './services/active-game.service';

@Component({
  selector: 'trpg-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent implements OnInit, OnDestroy {
  players: Player[] = [];
  isOwner = false;
  gameId = '';
  joined = false;

  constructor(private activatedRoute: ActivatedRoute, private gameService: GameService, private activeGameService: ActiveGameService) {
  }

  ngOnInit() {
    const id = this.activatedRoute.snapshot.params['id'];

    this.gameId = id;
    this.activeGameService.activeGameId = id;

    this.activeGameService.setup();

    this.gameService.joinGame()
      .subscribe((isOwner) => {
        this.activeGameService.updateIsOwner(isOwner);
        this.initializeGame();
      });
  }

  ngOnDestroy(){
    this.activeGameService.dispose();
  }

  initializeGame() {
    this.joined = true;
    this.gameService.getPlayers()
      .subscribe((players) => {
        this.activeGameService.updatePlayers(players);
      });
  }
}
