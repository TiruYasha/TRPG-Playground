import { Injectable } from '@angular/core';
import { Guid } from 'src/app/shared/utilities/guid.util';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { Player } from '../models/game/player.model';

@Injectable({
    providedIn: 'root'
})
export class GameHubService {
    private _hubConnection: HubConnection;

    public activeGameId = Guid.getEmptyGuid();

    constructor() { }

    setup() {
        this.createConnection();
        this.startConnection();
        this.registerOnServerEvents();
    }

    dispose() {
        this.hubConnection.stop();
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
