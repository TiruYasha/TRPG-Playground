import { Injectable } from '@angular/core';
import { JournalItem } from '../models/journal/journalitems/journal-item.model';

@Injectable({
    providedIn: 'root'
})
export class DragService {
    itemBeingDragged: JournalItem;
}
