import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Token } from '../models/play-area/token.model';

@Injectable({
  providedIn: 'root'
})
export class LayerService {
  constructor(private http: HttpClient) { }

  addToken(token: Token, layerId: string) {
    return this.http.post<Token>(environment.apiUrl + `/layer/${layerId}/token`, token);
  }
}
