import { JournalItem } from '../journalitems/journal-item.model';
import { Guid } from 'src/app/utilities/guid.util';

export class AddJournalItemRequestModel {
    journalItem: JournalItem;
    parentFolderId?: string = Guid.getEmptyGuid();
}
