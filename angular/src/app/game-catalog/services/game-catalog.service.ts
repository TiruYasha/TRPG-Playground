import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CreateGameModel } from 'src/app/shared/models/game-catalog/creategame.model';
import { GameCatalogItem } from 'src/app/shared/models/game-catalog/game-catalog-item.model';

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

  getGames(): Observable<GameCatalogItem[]> {
    return this.http.get<GameCatalogItem[]>(environment.apiUrl + '/game/all');
  }
}
