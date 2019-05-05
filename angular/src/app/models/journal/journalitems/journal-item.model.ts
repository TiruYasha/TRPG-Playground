import { JournalItemType } from './journal-item-type.enum';

export class JournalItem {
    id: string;
    name: string;
    type: JournalItemType;
    imageId?: string;
    canSee?: string[];
    canEdit?: string[];

    constructor(type: JournalItemType){
        this.type = type;
        this.canSee = [];
        this.canEdit = [];
    }
}
