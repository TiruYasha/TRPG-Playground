import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DefaultToken } from '../models/play-area/default-token.model';
import { DtoTokenFactory } from '../models/play-area/backend/dto-token-factory';
import { DefaultTokenDto } from '../models/play-area/backend/default-token.dto';
import { map } from 'rxjs/operators';
import { TokenFactory } from '../models/play-area/token-factory';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LayerService {
  constructor(private http: HttpClient) { }

  addToken(token: DefaultToken, layerId: string): Observable<DefaultToken> {
    console.log(`token: `, token);


    const dto = DtoTokenFactory.create(token);
    console.log(`dtotoken: `, dto);
    return this.http.post<DefaultTokenDto>(environment.apiUrl + `/layer/${layerId}/token`, dto)
      .pipe(map(res => TokenFactory.createFromDto(res)));
  }
}
