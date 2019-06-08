import { Subject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Injectable } from '@angular/core';
import { AddJournalItemRequestModel } from 'src/app/models/journal/requests/add-journal-folder-request.model';
import { AddedJournalItemModel } from 'src/app/models/journal/receives/added-journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { ActiveGameService } from '../services/active-game.service';
import { UploadedImage as UploadedJournalItemImage } from 'src/app/models/journal/receives/uploaded-image.model';
import { JournalTreeItem } from 'src/app/models/journal/journal-tree-item.model';
import { JournalEvents } from 'src/app/models/journal/journal-events.enum';

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

    constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

    setup() {
        this.registerOnServerEvents();
    }

    addJournalItemToGame(model: AddJournalItemRequestModel) {
        return this.http.post<AddedJournalItemModel>(environment.apiUrl + '/journal/AddJournalItem', model);
    }

    saveJournalItem(model: JournalItem) {
        return this.http.put(environment.apiUrl + '/journal/updateJournalItem', model);
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
    }
}
