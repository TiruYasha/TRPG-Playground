import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Player } from 'src/app/models/game/player.model';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';
import { Guid } from 'src/app/utilities/guid.util';

@Component({
  selector: 'trpg-create-handout-dialog',
  templateUrl: './create-handout-dialog.component.html',
  styleUrls: ['./create-handout-dialog.component.scss']
})
export class CreateHandoutDialogComponent {
  @Input() players: Player[];
  @Input() data: JournalHandout;
  @Input() isOwner: boolean;

  @Output() journalItem = new EventEmitter<JournalHandout>();

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

  get name() { return this.form.get('name'); }
  get description() { return this.form.get('description'); }
  get ownerNotes() { return this.form.get('ownerNotes'); }
  get canSee() { return this.form.get('canSee'); }
  get canEdit() { return this.form.get('canEdit'); }
  get image() { return this.form.get('image'); }

  save() {
    const canSee = (this.canSee.value as Player[]).map(m => m.userId);
    const canEdit = (this.canEdit.value as Player[]).map(m => m.userId);
    const handout = new JournalHandout();
    handout.name = this.name.value;
    handout.description = this.description.value;
    handout.ownerNotes = this.ownerNotes.value;
    handout.canSee = canSee;
    handout.canEdit = canEdit;
    handout.image = this.image.value;

    this.journalItem.emit(handout);
  }

  onImageChange(event: Event) {
    const target = event.target as HTMLInputElement;

    if (target.files && target.files.length) {
      const file = target.files[0];
      this.image.setValue(file);
    }
  }
}
