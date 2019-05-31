import { Player } from 'src/app/models/game/player.model';
import { DialogState } from './dialog-state.enum';
import { JournalItemType } from 'src/app/models/journal/journalitems/journal-item-type.enum';

export class ParentDialogModel {
    players: Player[];
    isOwner: boolean;
    state: DialogState;
    journalItemType: JournalItemType;
    parentFolderId?: string;
    journalItemId?: string;

}
