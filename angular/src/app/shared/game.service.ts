import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { Game } from '../models/game.model';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(private http: HttpClient) { }

  createGame(game: Game): Observable<Game> {
    console.log('game', game);
    return this.http.post<Game>(environment.apiUrl + '/api/game', game);
  }

  joinGame(id: string) {
    const game = new Game();
    game.id = id;
    return this.http.put(environment.apiUrl + '/api/game', game);
  }

  getGames() {
    return this.http.get<Game[]>(environment.apiUrl + '/api/game');
  }
}
