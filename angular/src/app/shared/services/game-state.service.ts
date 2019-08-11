import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { GameHubService } from './game-hub.service';
import { Player } from '../models/game/player.model';
import { PlayMap } from '../models/map/map.model';
import { GameEvents } from '../models/game/game-events.enum';
import { Layer } from '../models/map/layer.model';

@Injectable({
    providedIn: 'root'
})
export class GameStateService {
    private playersSubject = new BehaviorSubject<Player[]>([]);
    playersObservable = this.playersSubject.asObservable();

    private isOwnerSubject = new BehaviorSubject<boolean>(false);
    isOwnerObservable = this.isOwnerSubject.asObservable();

    private selectMapSubject = new Subject<PlayMap>();
    selectMapObservable = this.selectMapSubject.asObservable();

    private selectLayerSubject = new Subject<Layer>();
    selectLayerObservable = this.selectLayerSubject.asObservable();

    private mapVisibilityChangedSubject = new Subject<PlayMap>();
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

    selectMap(map: PlayMap) {
        this.selectMapSubject.next(map);
    }

    selectLayer(layer: Layer) {
        this.selectLayerSubject.next(layer);
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
