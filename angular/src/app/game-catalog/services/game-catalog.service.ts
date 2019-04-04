import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { CreateGameModel } from 'src/app/models/game-catalog/creategame.model';
import { Game } from 'src/app/models/game.model';
import { GameCatalogItem } from 'src/app/models/game-catalog/game-catalog-item.model';

@Injectable({
  providedIn: 'root'
})
export class GameCatalogService {
  constructor(private http: HttpClient) { }

  createGame(game: CreateGameModel): Observable<string> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
    const options = { headers, responseType: 'text' as 'json' };
    return this.http.post<string>(environment.apiUrl + '/game/create', game, options);
  }

  joinGame(id: string) {
    const game = new Game();
    game.id = id;
    return this.http.put(environment.apiUrl + '/game/join', {gameId: id});
  }

  getGames(): Observable<GameCatalogItem[]> {
    return this.http.get<GameCatalogItem[]>(environment.apiUrl + '/game/all');
  }
}