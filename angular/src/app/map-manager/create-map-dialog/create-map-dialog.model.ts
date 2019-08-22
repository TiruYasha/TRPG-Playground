import { PlayMap } from 'src/app/shared/models/map/map.model';
import { DialogState } from 'src/app/shared/models/dialog-state.enum';

export interface CreateMapDialogModel {
    map?: PlayMap;
    dialogState: DialogState;
}
