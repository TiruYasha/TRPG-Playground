import { Subject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { AddJournalItemRequestModel } from 'src/app/models/journal/requests/add-journal-folder-request.model';
import { AddedJournalItemModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { ActiveGameService } from '../services/active-game.service';

@Injectable({
    providedIn: 'root'
})
export class JournalService {
    private journalItemAddedSubject = new Subject<AddedJournalItemModel>();
    journalItemAdded = this.journalItemAddedSubject.asObservable();

    constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

    setup() {
        this.registerOnServerEvents();
    }

    addJournalItemToGame(model: AddJournalItemRequestModel) {
        return this.http.post<AddedJournalItemModel>(environment.apiUrl + '/journal/AddJournalItem', model);
    }

    getAllJournalItems(): Observable<JournalItem[]> {
        return this.http.get<JournalItem[]>(environment.apiUrl + '/journal/all');
    }

    getJournalItemsByParentFolderId(parentFolderId: string) {
        return this.http.get<JournalItem[]>(environment.apiUrl + `/journal/folder/${parentFolderId}/item`);
    }

    getRootJournalItems() {
        return this.http.get<JournalItem[]>(environment.apiUrl + `/journal/item`);
    }

    uploadImage(journalItemId: string, image: File) {
        const formData = new FormData();
        formData.append('file', image);
        return this.http.post(environment.apiUrl + `/journal/${journalItemId}/image`, formData);
    }

    private registerOnServerEvents(): void {
        this.activeGameService.hubConnection.on('JournalItemAdded', (data: AddedJournalItemModel) => {
            this.journalItemAddedSubject.next(data);
        });
    }
}
