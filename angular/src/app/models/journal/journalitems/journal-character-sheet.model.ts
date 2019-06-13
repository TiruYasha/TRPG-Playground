import { JournalItem } from './journal-item.model';
import { JournalItemType } from './journal-item-type.enum';

export class JournalCharacterSheet extends JournalItem {

    description?: string;
    ownerNotes?: string;
    tokenId?: string;
    token?: File;

    constructor() {
        super(JournalItemType.Handout);

    }
}
