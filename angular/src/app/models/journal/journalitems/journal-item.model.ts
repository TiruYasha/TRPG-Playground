import { JournalItemType } from './journal-item-type.enum';

export class JournalItem {
    id: string;
    name: string;
    type: JournalItemType;
    imageId?: string;

    constructor(type: JournalItemType){
        this.type = type;
    }
}
