import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { Game } from '../models/game.model';
import { CreateGameModel } from '../models/game-catalog/creategame.model';
import { Player } from '../models/game/player.model';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(private http: HttpClient) { }

  joinGame(): Observable<boolean> {
    return this.http.put<boolean>(environment.apiUrl + '/game/join', {});
  }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(environment.apiUrl + '/game/players');
  }
}
