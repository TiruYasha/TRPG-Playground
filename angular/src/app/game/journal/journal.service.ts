import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { AddJournalFolderRequestModel } from 'src/app/models/journal/requests/AddJournalFolderRequest.model';
import { AddedJournalFolderModel } from 'src/app/models/journal/receives/added-journal-folder.model';

@Injectable({
    providedIn: 'root'
})
export class JournalService {
    AddedJournalFolder = new Subject();

    private hubConnection: HubConnection;

    constructor(private http: HttpClient) { }

    setup(gameId: string) {
        this.createConnection();
        this.registerOnServerEvents();
        this.startConnection(gameId);
    }

    addToGroup(gameId: string): any {
        this.hubConnection.invoke('AddToGroup', gameId);
    }

    addFolderToGame(model: AddJournalFolderRequestModel) {
        this.hubConnection.invoke('AddJournalFolderAsync', model);
    }

    private createConnection() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.apiUrl + '/journalhub', { accessTokenFactory: this.getAccessToken })
            .build();
    }

    private startConnection(gameId: string): void {
        this.hubConnection
            .start()
            .then(() => {
                this.addToGroup(gameId);
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
        // TODO change any!!!
        this.hubConnection.on('AddedJournalFolder', (data: AddedJournalFolderModel) => {
            console.log('received message: ', data);
            this.AddedJournalFolder.next(data);
        });
    }
}
