import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { AddJournalFolderRequestModel } from 'src/app/models/journal/requests/AddJournalFolderRequest.model';
import { AddedJournalFolderModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { ActiveGameService } from '../services/active-game.service';

@Injectable({
    providedIn: 'root'
})
export class JournalService {
    AddedJournalFolder = new Subject();
    AddedToGroup = new Subject();

    private hubConnection: HubConnection;

    constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

    setup() {
        this.createConnection();
        this.registerOnServerEvents();
        this.startConnection();
    }

    addToGroup(): any {
        this.hubConnection.invoke('AddToGroup', this.activeGameService.activeGameId);
    }

    addFolderToGame(model: AddJournalFolderRequestModel) {
        this.hubConnection.invoke('AddJournalFolderAsync', model);
    }

    private createConnection() {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.apiUrl + '/journalhub', {
                accessTokenFactory: this.getAccessToken,
            })
            .build();
    }

    private startConnection(): void {
        this.hubConnection
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

        this.hubConnection.on('AddedJournalFolder', (data: AddedJournalFolderModel) => {
            this.AddedJournalFolder.next(data);
        });

        this.hubConnection.on('AddedToGroup', (data: JournalItem[]) => {
            this.AddedToGroup.next(data);
        });
    }
}
