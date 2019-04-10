import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { HttpClient } from '@angular/common/http';
import { ActiveGameService } from '../services/active-game.service';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class JournalService {
    AddedJournalFolder = new Subject();

    private _hubConnection: HubConnection;

    constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

    setup() {
        this.createConnection();
        this.registerOnServerEvents();
        this.startConnection();
    }

    addToGroup(): any {
        this._hubConnection.invoke('AddToGroup', this.activeGameService.gameId);
    }

    private createConnection() {
        this._hubConnection = new HubConnectionBuilder()
            .withUrl(environment.apiUrl + '/chathub', { accessTokenFactory: this.getAccessToken })
            .build();
    }

    private startConnection(): void {
        this._hubConnection
            .start()
            .then(() => {
                this.addToGroup();
            })
            .catch(err => {
                console.log('Error while establishing connection, retrying...', err);
                setTimeout(this.startConnection, 5000);
            });
    }


    private getAccessToken() {
        return localStorage.getItem('token');
    }

    private registerOnServerEvents(): void {
        //   this._hubConnection.on('ReceiveMessage', (data: CommandResult) => {
        //     console.log('received message: ', data);
        //     this.receivedMessage.next(data);
        //   });
    }


}
