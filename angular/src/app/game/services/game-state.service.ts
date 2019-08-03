import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { Player } from 'src/app/models/game/player.model';
import { PlayMap } from 'src/app/models/map/map.model';
import { GameHubService } from './game-hub.service';
import { GameEvents } from 'src/app/models/game/game-events.enum';

@Injectable({
    providedIn: 'root'
})
export class GameStateService {
    private playersSubject = new BehaviorSubject<Player[]>([]);
    playersObservable = this.playersSubject.asObservable();

    private isOwnerSubject = new BehaviorSubject<boolean>(false);
    isOwnerObservable = this.isOwnerSubject.asObservable();

    private changeMapSubject = new Subject<PlayMap>();
    changeMapObservable = this.changeMapSubject.asObservable();

    private mapVisibilityChangedSubject = new BehaviorSubject<PlayMap>(new PlayMap());
    mapVisibilityChangedObservable = this.mapVisibilityChangedSubject.asObservable();

    constructor(private gameHubService: GameHubService) {

    }

    setup() {
        this.registerOnServerEvents();
    }

    updatePlayers(players: Player[]) {
        this.playersSubject.next(players);
    }

    updateIsOwner(isOwner: boolean) {
        this.isOwnerSubject.next(isOwner);
    }

    changeMap(map: PlayMap) {
        this.changeMapSubject.next(map);
    }

    changeVisibleMap(map: PlayMap) {
        this.mapVisibilityChangedSubject.next(map);
    }

    private registerOnServerEvents(): void {
        this.gameHubService.hubConnection.on(GameEvents.MapVisibilityChanged, (data: PlayMap) => {
            this.mapVisibilityChangedSubject.next(data);
        });
    }
}
