import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders
} from '@angular/common/http';

import { Observable } from 'rxjs';
import { GameHubService } from '../game/services/game-hub.service';

/** Pass untouched request through to the next request handler. */
@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private activeGameService: GameHubService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const headers = new HttpHeaders(
            {
                'Authorization': 'Bearer ' + localStorage.getItem('token'),
                'GameId': this.activeGameService.activeGameId
            });

        req = req.clone({
            headers: headers
        });

        return next.handle(req);
    }
}
