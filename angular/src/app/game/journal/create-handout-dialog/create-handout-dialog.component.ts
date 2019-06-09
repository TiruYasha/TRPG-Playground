import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Player } from 'src/app/models/game/player.model';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';
import { DialogState } from '../parent-dialog/dialog-state.enum';
import { environment } from 'src/environments/environment';
import { JournalItemPermission } from 'src/app/models/journal/journalitems/journal-item-permission.model';

@Component({
  selector: 'trpg-create-handout-dialog',
  templateUrl: './create-handout-dialog.component.html',
  styleUrls: ['./create-handout-dialog.component.scss']
})
export class CreateHandoutDialogComponent implements OnInit {


  @Input() players: Player[];
  @Input() data: JournalHandout;
  @Input() isOwner: boolean;
  @Input() dialogState: DialogState;

  @Output() journalItem = new EventEmitter<JournalHandout>();

  states = DialogState;

  form = new FormGroup({
    name: new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]),
    canSee: new FormControl([]),
    canEdit: new FormControl([]),
    description: new FormControl(''),
    ownerNotes: new FormControl(''),
    image: new FormControl(null)
  });

  constructor() {
  }

  ngOnInit(): void {
    if (this.dialogState === DialogState.Edit) {
      this.name.setValue(this.data.name);
      this.setPermissionInputValues();
      this.description.setValue(this.data.description);
      this.ownerNotes.setValue(this.data.ownerNotes);
    }
  }

  private setPermissionInputValues() {
    const canSeeValues: string[]  = [];
    const canEditValues: string[]  = [];
    this.data.permissions.forEach(permission => {
      if (permission.canEdit) {
        canSeeValues.push(permission.userId);
        canEditValues.push(permission.userId);
      } else if (permission.canSee) {
        canSeeValues.push(permission.userId);
      }
    });
    this.canSee.setValue(canSeeValues);
    this.canEdit.setValue(canEditValues);
  }

  get name() { return this.form.get('name'); }
  get description() { return this.form.get('description'); }
  get ownerNotes() { return this.form.get('ownerNotes'); }
  get canSee() { return this.form.get('canSee'); }
  get canEdit() { return this.form.get('canEdit'); }
  get image() { return this.form.get('image'); }

  save() {
    const handout = new JournalHandout();
    handout.name = this.name.value;
    handout.description = this.description.value;
    handout.ownerNotes = this.ownerNotes.value;
    handout.permissions = this.createPermissions();
    handout.image = this.image.value;
    this.journalItem.emit(handout);
  }

  private createPermissions() {
    const permissions: JournalItemPermission[] = [];
    this.canEdit.value.forEach(element => {
      permissions.push({
        canEdit: true,
        canSee: true,
        userId: element
      });
    });
    this.canSee.value.forEach(element => {
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

  onImageChange(event: Event) {
    const target = event.target as HTMLInputElement;

    if (target.files && target.files.length) {
      const file = target.files[0];
      this.image.setValue(file);
    }
  }

  getImageLink(journalItemId: string) {
    return `${environment.apiUrl}/journal/${journalItemId}/image`;
  }
}
