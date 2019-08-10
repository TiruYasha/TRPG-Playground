import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Player } from './models/game/player.model';
import { InitialGameData } from './models/game/initial-game-data.model';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(private http: HttpClient) { }

  joinGame(): Observable<boolean> {
    return this.http.put<boolean>(environment.apiUrl + '/game/join', {});
  }

  getInitialGameData(): Observable<InitialGameData> {
    return this.http.get<InitialGameData>(environment.apiUrl + '/game/initial', {});
  }

  getPlayers(): Observable<Player[]> {
    return this.http.get<Player[]>(environment.apiUrl + '/game/players');
  }
}
