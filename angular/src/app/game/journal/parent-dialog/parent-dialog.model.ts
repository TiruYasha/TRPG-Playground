import { Player } from 'src/app/models/game/player.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { Guid } from 'src/app/utilities/guid.util';

export class ParentDialogModel {
    players: Player[];
    data?: JournalItem;
    isOwner: boolean;
    parentFolderId?: string = Guid.getEmptyGuid();
}
