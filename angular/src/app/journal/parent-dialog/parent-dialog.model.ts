import { Player } from 'src/app/shared/models/game/player.model';
import { JournalItemType } from 'src/app/shared/models/journal/journalitems/journal-item-type.enum';
import { DialogState } from 'src/app/shared/models/dialog-state.enum';

export class ParentDialogModel {
    players: Player[];
    isOwner: boolean;
    state: DialogState;
    journalItemType: JournalItemType;
    parentFolderId?: string;
    journalItemId?: string;
    canEdit: boolean;
}
