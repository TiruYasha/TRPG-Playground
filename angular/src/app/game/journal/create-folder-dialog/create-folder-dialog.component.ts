import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ParentDialogComponent } from '../parent-dialog/parent-dialog.component';
import { FormControl, Validators } from '@angular/forms';
import { DialogModel } from '../parent-dialog/dialog.model';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { OuterSubscriber } from 'rxjs/internal/OuterSubscriber';
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
