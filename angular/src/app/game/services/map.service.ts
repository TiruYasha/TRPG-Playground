import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AddMap } from 'src/app/models/map/requests/add-map.model';
import { PlayMap } from 'src/app/models/map/map.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MapService {
    constructor(private http: HttpClient) { }

    addMap(model: AddMap) {
        return this.http.post<PlayMap>(environment.apiUrl + `/game/map`, model);
    }

    getAllMaps(): Observable<PlayMap[]> {
        return this.http.get<PlayMap[]>(environment.apiUrl + `/game/map`);
    }
}
