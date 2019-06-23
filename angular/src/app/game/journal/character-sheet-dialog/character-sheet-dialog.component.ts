import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { DialogState } from '../../../models/dialog-state.enum';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { PermissionParser } from 'src/app/utilities/permission-parser.util';
import { takeUntil } from 'rxjs/operators';
import { JournalCharacterSheet } from 'src/app/models/journal/journalitems/journal-character-sheet.model';
import { environment } from 'src/environments/environment';
import { Player } from 'src/app/models/game/player.model';

@Component({
  selector: 'trpg-character-sheet-dialog',
  templateUrl: './character-sheet-dialog.component.html',
  styleUrls: ['./character-sheet-dialog.component.scss']
})
export class CharacterSheetDialogComponent extends DestroySubscription implements OnInit {

  @Input() players: Player[];
  @Input() data: JournalCharacterSheet;
  @Input() isOwner: boolean;
  @Input() dialogState: DialogState;

  @Output() isValid = new EventEmitter<boolean>();
  @Output() changeImage = new EventEmitter<File>();
  @Output() changeToken = new EventEmitter<File>();

  states = DialogState;

  imageToUpload: string = null;
  tokenToUpload: string = null;

  form = new FormGroup({
    name: new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]),
    canSee: new FormControl([]),
    canEdit: new FormControl([]),
    description: new FormControl(''),
    ownerNotes: new FormControl(''),
    image: new FormControl(null),
    token: new FormControl(null)
  });

  get name() { return this.form.get('name'); }
  get description() { return this.form.get('description'); }
  get ownerNotes() { return this.form.get('ownerNotes'); }
  get canSee() { return this.form.get('canSee'); }
  get canEdit() { return this.form.get('canEdit'); }
  get image() { return this.form.get('image'); }
  get token() { return this.form.get('token'); }

  constructor() {
    super();
  }

  ngOnInit() {
    this.name.setValue(this.data.name);
    this.setPermissionInputValues();
    this.description.setValue(this.data.description);
    this.ownerNotes.setValue(this.data.ownerNotes);

    this.form.valueChanges.pipe(takeUntil(this.destroy))
      .subscribe(d => {
        this.isValid.emit(this.form.valid);

        if (this.form.valid) {
          this.data.name = this.name.value;
          this.data.description = this.description.value;
          this.data.ownerNotes = this.ownerNotes.value;
          this.data.permissions = PermissionParser.createPermissions(this.canEdit.value, this.canSee.value);
          this.data.image = this.image.value;
          this.data.token = this.token.value;
        }
      });
  }

  onImageChange(event: Event) {
    const target = event.target as HTMLInputElement;

    if (target.files && target.files.length) {
      const file = target.files[0];
      this.image.setValue(file);
      this.changeImage.emit(file);
      const reader = new FileReader();
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.imageToUpload = reader.result.toString();
      };
    }
  }

  onTokenChange(event: Event) {
    const target = event.target as HTMLInputElement;

    if (target.files && target.files.length) {
      const file = target.files[0];
      this.token.setValue(file);
      this.changeToken.emit(file);
      const reader = new FileReader();
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.tokenToUpload = reader.result.toString();
      };
    }
  }

  getImageLink(journalItemId: string) {
    return `${environment.apiUrl}/journal/${journalItemId}/image`;
  }

  getTokenLink(journalItemId: string) {
    return `${environment.apiUrl}/journal/${journalItemId}/token`;
  }

  private setPermissionInputValues() {
    const [canSeeValues, canEditValues] = PermissionParser.splitJournalItemPermissions(this.data.permissions);

    this.canSee.setValue(canSeeValues);
    this.canEdit.setValue(canEditValues);
  }
}
