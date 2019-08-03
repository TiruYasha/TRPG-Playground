import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GameService } from '../shared/game.service';
import { Player } from '../models/game/player.model';
import { GameHubService } from './services/game-hub.service';
import { takeUntil } from 'rxjs/operators';
import { DestroySubscription } from '../shared/components/destroy-subscription.extendable';
import { GameStateService } from './services/game-state.service';

@Component({
  selector: 'trpg-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.scss']
})
export class GameComponent extends DestroySubscription implements OnInit, OnDestroy {
  players: Player[] = [];
  isOwner = false;
  joined = false;

  constructor(private activatedRoute: ActivatedRoute,
    private gameService: GameService,
    private gameHubService: GameHubService,
    private gameState: GameStateService) {
    super();
  }

  ngOnInit() {
    const id = this.activatedRoute.snapshot.params['id'];
    this.gameHubService.activeGameId = id;

    this.gameHubService.setup();

    this.gameService.joinGame()
      .pipe(takeUntil(this.destroy))
      .subscribe((isOwner) => {
        this.initializeGame();
      });
  }

  ngOnDestroy() {
    this.gameHubService.dispose();
  }

  initializeGame() {
    this.joined = true;
    this.gameService.getInitialGameData()
      .pipe(takeUntil(this.destroy))
      .subscribe(gameData => {
        this.isOwner = gameData.isOwner;
        this.gameState.setup();
        this.gameState.updateIsOwner(gameData.isOwner);
        this.gameState.updatePlayers(gameData.players);

        if (gameData.visibleMap) {
          this.gameState.changeVisibleMap(gameData.visibleMap);
        }
      });
  }
}
