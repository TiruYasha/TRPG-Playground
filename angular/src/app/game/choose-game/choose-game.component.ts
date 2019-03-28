import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/models/game.model';
import { GameService } from 'src/app/shared/game.service';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-choose-game',
  templateUrl: './choose-game.component.html',
  styleUrls: ['./choose-game.component.css']
})
export class ChooseGameComponent implements OnInit {

  createGameName = '';

  games: Game[] = [];

  constructor(private gameService: GameService, private router: Router) { }

  ngOnInit() {
    this.gameService.getGames().subscribe(data => {
      this.games = data;
    });
  }

  createGame() {
    const game: Game = {
      id: '877ae3ab-16f7-4262-8009-675e16630b3b',
      name: this.createGameName
    };

    this.gameService.createGame(game).subscribe(data => {
      console.log('Created game: ', data);
      this.router.navigate(['/game/' + data.id]);
    });
  }

  joinGame(id) {
    this.gameService.joinGame(id).subscribe(data => {
      console.log('join successfull');
      this.router.navigate(['/game/' + id]);
    });
  }
}
