import { JournalItemType } from './journalitems/journal-item-type.enum';

export class JournalTreeItem {
    id: string;
    name: string;
    parentFolderId?: string;
    type: JournalItemType;
    imageId?: string;
    canEdit: boolean;
}
