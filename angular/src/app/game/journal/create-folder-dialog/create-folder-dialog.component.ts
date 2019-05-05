import { Component, Output, EventEmitter } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';

@Component({
  selector: 'trpg-create-folder-dialog',
  templateUrl: './create-folder-dialog.component.html'
})
export class CreateFolderDialogComponent {
  @Output() journalItem = new EventEmitter<JournalItem>();
  @Output() close = new EventEmitter();

  name = new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]);

  constructor() { }

  onNoClick(): void {
    this.close.emit();
  }

  Ok() {
    const folder = new JournalFolder();
    folder.name = this.name.value;
    this.journalItem.emit(folder);
  }
}
