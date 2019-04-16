import { Injectable } from '@angular/core';
import { Guid } from 'src/app/utilities/guid.util';
import { BehaviorSubject } from 'rxjs';
import { Player } from 'src/app/models/game/player.model';

@Injectable({
    providedIn: 'root'
})
export class ActiveGameService {
    public activeGameId = Guid.getEmptyGuid();

    private playersSubject = new BehaviorSubject<Player[]>([]);
    private isOwnerSubject = new BehaviorSubject<boolean>(false);

    public playersObservable = this.playersSubject.asObservable();
    public isOwnerObservable = this.isOwnerSubject.asObservable();
    
    public updatePlayers(players: Player[]){
        this.playersSubject.next(players);
    }

    public updateIsOwner(isOwner: boolean){
        this.isOwnerSubject.next(isOwner);
    }
}
