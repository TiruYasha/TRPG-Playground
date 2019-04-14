import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { Game } from '../models/game.model';
import { CreateGameModel } from '../models/game-catalog/creategame.model';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(private http: HttpClient) { }

  createGame(game: CreateGameModel): Observable<any> {
    return this.http.post(environment.apiUrl + '/game/create', game);
  }

  joinGame(): Observable<boolean> {
    return this.http.put<boolean>(environment.apiUrl + '/game/join', {});
  }

  getGames() {
    return this.http.get<Game[]>(environment.apiUrl + '/game/all');
  }
}
