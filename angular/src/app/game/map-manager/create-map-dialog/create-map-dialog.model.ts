import { PlayMap } from 'src/app/models/map/map.model';
import { DialogState } from 'src/app/models/dialog-state.enum';

export interface CreateMapDialogModel {
    map?: PlayMap;
    dialogState: DialogState;
}
