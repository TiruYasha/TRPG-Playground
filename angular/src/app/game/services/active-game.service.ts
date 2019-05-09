import { Injectable } from '@angular/core';
import { Guid } from 'src/app/utilities/guid.util';
import { BehaviorSubject } from 'rxjs';
import { Player } from 'src/app/models/game/player.model';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ActiveGameService {
    private _hubConnection: HubConnection;

    public activeGameId = Guid.getEmptyGuid();

    private playersSubject = new BehaviorSubject<Player[]>([]);

    private isOwnerSubject = new BehaviorSubject<boolean>(false);

    public playersObservable = this.playersSubject.asObservable();
    public isOwnerObservable = this.isOwnerSubject.asObservable();

    constructor() { }

    setup() {
        this.createConnection();
        this.startConnection();
        this.registerOnServerEvents();
    }

    registerOnServerEvents() {
        this.hubConnection.on('ActivePlayerJoined', (data: string) => {

        });

        this.hubConnection.on('ActivePlayerLeft', (data: string) => {

        });

        this.hubConnection.on('PlayerJoined', (data: Player) => {

        });
    }

    get hubConnection(): HubConnection {
        return this._hubConnection;
    }

    public updatePlayers(players: Player[]) {
        this.playersSubject.next(players);
    }

    public updateIsOwner(isOwner: boolean) {
        this.isOwnerSubject.next(isOwner);
    }

    private startConnection() {
        this._hubConnection
            .start()
            .then(() => {
                this.addToGroup();
            })
            .catch(err => {
                console.log('Error while establishing connection,  retrying...', err);
                setTimeout(this.startConnection, 4000);
            });
    }

    private addToGroup() {
        this._hubConnection.invoke('AddToGroup', this.activeGameId);
    }

    private createConnection() {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(environment.apiUrl + '/gamehub', {
                accessTokenFactory: this.getAccessToken
            })
            .build();
    }

    private getAccessToken() {
        return localStorage.getItem('token');
    }
}
