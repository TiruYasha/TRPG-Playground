import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AddMap } from 'src/app/models/map/requests/add-map.model';
import { PlayMap } from 'src/app/models/map/map.model';
import { environment } from 'src/environments/environment';
import { Observable, Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MapService {
    private changeMapSubject = new Subject<PlayMap>();
    changeMapObservable = this.changeMapSubject.asObservable();

    constructor(private http: HttpClient) { }

    addMap(model: AddMap) {
        return this.http.post<PlayMap>(environment.apiUrl + `/game/map`, model);
    }

    getAllMaps(): Observable<PlayMap[]> {
        return this.http.get<PlayMap[]>(environment.apiUrl + `/game/map`);
    }

    deleteMap(mapId: string): Observable<void> {
        return this.http.delete<void>(environment.apiUrl + `/map/${mapId}`);
    }

    updateMap(map: PlayMap): Observable<void>  {
        return this.http.put<void>(environment.apiUrl + `/map`, map);
    }

    changeMap(map: PlayMap) {
        this.changeMapSubject.next(map);
    }
}
