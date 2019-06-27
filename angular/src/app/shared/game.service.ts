import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Player } from '../models/game/player.model';
import { AddMap } from '../models/map/requests/add-map.model';
import { PlayMap } from '../models/map/map.model';

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
