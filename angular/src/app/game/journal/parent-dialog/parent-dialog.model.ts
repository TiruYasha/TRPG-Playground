import { Player } from 'src/app/models/game/player.model';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';

export class ParentDialogModel<T> {
    players: Player[];
    journalItemType: JournalItemType;
    data: T;
}
