import { JournalItemType } from '../journalitems/journal-item-type.enum';

export class AddedJournalItemModel {
    id: string;
    name: string;
    parentId: string;
    imageId: string;
    type: JournalItemType;

}
