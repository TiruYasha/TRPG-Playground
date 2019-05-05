import { JournalItem } from './journal-item.model';
import { JournalItemType } from './journal-item-type.enum';

export class JournalHandout extends JournalItem {

    description?: string;
    ownerNotes?: string;
    image?: File;

    constructor() {
        super(JournalItemType.Handout);

    }
}
