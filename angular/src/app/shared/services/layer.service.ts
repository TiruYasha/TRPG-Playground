import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DefaultToken } from '../models/play-area/default-token.model';

@Injectable({
  providedIn: 'root'
})
export class LayerService {
  constructor(private http: HttpClient) { }

  addToken(token: DefaultToken, layerId: string) {
    return this.http.post<DefaultToken>(environment.apiUrl + `/layer/${layerId}/token`, token);
  }
}
