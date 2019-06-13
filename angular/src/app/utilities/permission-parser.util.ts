import { JournalItemPermission } from '../models/journal/journalitems/journal-item-permission.model';

export class PermissionParser {
    public static createPermissions(canEdit: string[], canSee: string[]) {
        const permissions: JournalItemPermission[] = [];
        canEdit.forEach(element => {
            permissions.push({
                canEdit: true,
                canSee: true,
                userId: element
            });
        });
        canSee.forEach(element => {
            const permission = permissions.filter(e => e.userId === element);
            if (permission.length === 0) {
                permissions.push({
                    canEdit: false,
                    canSee: true,
                    userId: element
                });
            }
        });

        return permissions;
    }

    public static splitJournalItemPermissions(permissions: JournalItemPermission[]): [string[], string[]] {
        const canSeeValues: string[] = [];
        const canEditValues: string[] = [];
        permissions.forEach(permission => {
          if (permission.canEdit) {
            canSeeValues.push(permission.userId);
            canEditValues.push(permission.userId);
          } else if (permission.canSee) {
            canSeeValues.push(permission.userId);
          }
        });

        return [canSeeValues, canEditValues];
      }
}
