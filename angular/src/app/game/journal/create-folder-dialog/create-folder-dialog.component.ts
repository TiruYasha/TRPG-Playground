import { Component, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/utilities/validator-functions';
import { JournalFolder } from 'src/app/models/journal/journalitems/journal-folder.model';
import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
import { DialogPosition } from '@angular/material';
import { DialogState } from '../parent-dialog/dialog-state.enum';

@Component({
  selector: 'trpg-create-folder-dialog',
  templateUrl: './create-folder-dialog.component.html'
})
export class CreateFolderDialogComponent implements OnInit {
  @Output() journalItem = new EventEmitter<JournalItem>();
  @Output() close = new EventEmitter();

  @Input() data: JournalFolder;
  @Input() state: DialogState;

  states = DialogState;
  name = new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]);

  constructor() { }

  ngOnInit(): void {
    if (this.state === DialogState.Edit) {
      this.name.setValue(this.data.name);
    }
  }

  onNoClick(): void {
    this.close.emit();
  }

  Ok() {
    const folder = new JournalFolder();
    folder.name = this.name.value;
    this.journalItem.emit(folder);
  }
}
