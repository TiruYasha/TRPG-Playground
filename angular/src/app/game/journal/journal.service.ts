import { Subject, Observable } from 'rxjs';
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
    private journalFolderAddedSubject = new Subject<AddedJournalFolderModel>();
    journalFolderAdded = this.journalFolderAddedSubject.asObservable();

    constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

    setup() {
        this.registerOnServerEvents();
    }

    addFolderToGame(model: AddJournalFolderRequestModel) {
        return this.http.post(environment.apiUrl + '/journal/addJournalFolder', model);
    }

    getAllJournalItems(): Observable<JournalItem[]> {
        return this.http.get<JournalItem[]>(environment.apiUrl + '/journal/all');
    }

    private registerOnServerEvents(): void {
        this.activeGameService.hubConnection.on('JournalFolderAdded', (data: AddedJournalFolderModel) => {
            this.journalFolderAddedSubject.next(data);
        });
    }
}
