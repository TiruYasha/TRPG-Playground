import { JournalItem } from './journal-item.model';
import { JournalItemType } from './journal-item-type.enum';

export class JournalHandout extends JournalItem {

    constructor() {
        super(JournalItemType.Handout);
    }
}
