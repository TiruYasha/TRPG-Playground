import { JournalItem } from '../journalitems/journal-item.model';

export class AddJournalItemRequestModel {
    journalItem: JournalItem;
    parentFolderId?: string;
}
