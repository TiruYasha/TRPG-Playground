import { JournalItemType } from '../journalitems/journal-item-type.enum';

export class AddedJournalItemModel {
    id: string;
    name: string;
    parentFolderId: string;
    imageId: string;
    type: JournalItemType;

}
