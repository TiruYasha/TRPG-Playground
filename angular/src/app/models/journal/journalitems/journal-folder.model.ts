import { JournalItem } from './journal-item.model';
import { JournalItemType } from './journal-item-type.enum';

export class JournalFolder extends JournalItem {
    public journalItems: JournalItem[];

    constructor() {
        super(JournalItemType.Folder);
        this.journalItems = [];
    }
}
