import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { CreateGameModel } from 'src/app/models/game/creategame.model';
import { Game } from 'src/app/models/game.model';

@Injectable({
  providedIn: 'root'
})
export class GameCatalogService {
  constructor(private http: HttpClient) { }

  createGame(game: CreateGameModel): Observable<any> {
    console.log(environment.apiUrl);
    return this.http.post(environment.apiUrl + '/game/create', game);
  }

  joinGame(id: string) {
    const game = new Game();
    game.id = id;
    return this.http.put(environment.apiUrl + '/game/join', {gameId: id});
  }

  getGames() {
    return this.http.get<Game[]>(environment.apiUrl + '/game/all');
  }
}
