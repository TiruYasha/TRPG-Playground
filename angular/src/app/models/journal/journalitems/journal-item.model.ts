import { JournalItemType } from './journal-item-type.enum';
import { JournalItemPermission } from './journal-item-permission.model';

export class JournalItem {
    id: string;
    name: string;
    type: JournalItemType;
    imageId?: string;
    image?: File;
    permissions?: JournalItemPermission[];
    parentFolderId?: string;

    constructor(type: JournalItemType) {
        this.type = type;
        this.permissions = [];
    }
}
