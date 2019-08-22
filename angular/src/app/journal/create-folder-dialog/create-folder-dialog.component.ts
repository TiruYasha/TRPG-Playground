import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ValidatorFunctions } from 'src/app/shared/utilities/validator-functions';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { takeUntil } from 'rxjs/operators';
import { JournalFolder } from 'src/app/shared/models/journal/journalitems/journal-folder.model';

@Component({
  selector: 'trpg-create-folder-dialog',
  templateUrl: './create-folder-dialog.component.html'
})
export class CreateFolderDialogComponent extends DestroySubscription implements OnInit {
  @Input() data: JournalFolder;

  @Output() isValid = new EventEmitter<boolean>();
  name = new FormControl('', [Validators.required, ValidatorFunctions.noWhitespaceValidator]);

  constructor() { super(); }

  ngOnInit(): void {
    this.name.setValue(this.data.name);
    this.name.valueChanges.pipe(takeUntil(this.destroy))
      .subscribe(() => {
        if (this.name.valid) {
          this.data.name = this.name.value;
          this.isValid.emit(this.name.valid);
        }
      });
  }
}