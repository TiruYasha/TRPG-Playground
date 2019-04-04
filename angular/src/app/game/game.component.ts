import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActiveGameService } from './services/active-game.service';

@Component({
  selector: 'trpg-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {

  constructor(private _activatedRoute: ActivatedRoute, private _activeGameService: ActiveGameService) {
   }

  ngOnInit() {
    const id = this._activatedRoute.snapshot.params['id'];

    this._activeGameService.gameId = id;
  }
}
