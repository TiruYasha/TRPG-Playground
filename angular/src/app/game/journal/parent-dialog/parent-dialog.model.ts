import { DialogState } from '../../../shared/models/dialog-state.enum';
import { Player } from 'src/app/shared/models/game/player.model';
import { JournalItemType } from 'src/app/shared/models/journal/journalitems/journal-item-type.enum';

export class ParentDialogModel {
    players: Player[];
    isOwner: boolean;
    state: DialogState;
    journalItemType: JournalItemType;
    parentFolderId?: string;
    journalItemId?: string;
    canEdit: boolean;
}
