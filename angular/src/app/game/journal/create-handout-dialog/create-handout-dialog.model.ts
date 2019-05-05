import { DialogModel } from '../parent-dialog/dialog.model';

export class HandoutDialogModel implements DialogModel {
    name: string;
    description?: string;
    canSee?: string[];
    canEdit?: string[];
    ownerNotes?: string;
    image?: File;
}
