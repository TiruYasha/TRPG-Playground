import { Subject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { GameHubService } from './game-hub.service';
import { AddedJournalItemModel } from '../models/journal/receives/added-journal-folder.model';
import { JournalTreeItem } from '../models/journal/journal-tree-item.model';
import { AddJournalItemRequestModel } from '../models/journal/requests/add-journal-item-request.model';
import { JournalItem } from '../models/journal/journalitems/journal-item.model';
import { JournalEvents } from '../models/journal/journal-events.enum';
import { UploadedJournalItemImage } from '../models/journal/receives/uploaded-image.model';

@Injectable({
    providedIn: 'root'
})
export class JournalService {
    private journalItemAddedSubject = new Subject<AddedJournalItemModel>();
    journalItemAdded = this.journalItemAddedSubject.asObservable();

    private journalItemImageUploadedSubject = new Subject<UploadedJournalItemImage>();
    journalItemImageUploaded = this.journalItemImageUploadedSubject.asObservable();

    private journalItemUpdatedSubject = new Subject<JournalTreeItem>();
    journalItemUpdated = this.journalItemUpdatedSubject.asObservable();

    private journalItemDeletedSubject = new Subject<string>();
    journalItemDeleted = this.journalItemDeletedSubject.asObservable();

    constructor(private http: HttpClient, private activeGameService: GameHubService) { }

    setup() {
        this.registerOnServerEvents();
    }

    addJournalItemToGame(model: AddJournalItemRequestModel) {
        return this.http.post<AddedJournalItemModel>(environment.apiUrl + '/journal/AddJournalItem', model);
    }

    saveJournalItem(model: JournalItem) {
        return this.http.put(environment.apiUrl + '/journal/updateJournalItem', model);
    }

    deleteJournalItem(journalItemId: string) {
        return this.http.delete(environment.apiUrl + '/journal/deleteJournalItem/' + journalItemId);
    }

    getJournalItemById(journalItemId): Observable<JournalItem> {
        return this.http.get<JournalItem>(environment.apiUrl + '/journal/' + journalItemId);
    }

    getAllJournalItems(): Observable<JournalItem[]> {
        return this.http.get<JournalItem[]>(environment.apiUrl + '/journal/all');
    }

    getJournalItemsByParentFolderId(parentFolderId: string) {
        return this.http.get<JournalTreeItem[]>(environment.apiUrl + `/journal/folder/${parentFolderId}/item`);
    }

    getRootJournalItems() {
        return this.http.get<JournalTreeItem[]>(environment.apiUrl + `/journal/item`);
    }

    uploadImage(journalItemId: string, image: File) {
        const formData = new FormData();
        formData.append('file', image);
        return this.http.post(environment.apiUrl + `/journal/${journalItemId}/image`, formData);
    }

    uploadToken(journalItemId: string, image: File) {
        const formData = new FormData();
        formData.append('file', image);
        return this.http.post(environment.apiUrl + `/journal/${journalItemId}/token`, formData);
    }

    private registerOnServerEvents(): void {
        this.activeGameService.hubConnection.on(JournalEvents.journalItemAdded, (data: AddedJournalItemModel) => {
            this.journalItemAddedSubject.next(data);
        });

        this.activeGameService.hubConnection.on(JournalEvents.journalItemImageUploaded, (data: UploadedJournalItemImage) => {
            this.journalItemImageUploadedSubject.next(data);
        });

        this.activeGameService.hubConnection.on(JournalEvents.journalItemUpdated, (data: JournalTreeItem) => {
            this.journalItemUpdatedSubject.next(data);
        });

        this.activeGameService.hubConnection.on(JournalEvents.journalItemDeleted, (journalItemId: string) => {
            this.journalItemDeletedSubject.next(journalItemId);
        });
    }
}
