import { Component, OnInit, Input, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/utilities/CustomValidators';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ParentDialogComponent } from '../parent-dialog/parent-dialog.component';
import { JournalHandout } from 'src/app/models/journal/journalitems/journal-handout.model';

@Component({
  selector: 'trpg-create-handout-dialog',
  templateUrl: './create-handout-dialog.component.html',
  styleUrls: ['./create-handout-dialog.component.scss']
})
export class CreateHandoutDialogComponent implements OnInit {
  name = new FormControl('', [Validators.required, CustomValidators.noWhitespaceValidator]);

  constructor(
    public dialogRef: MatDialogRef<ParentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: JournalHandout
  ) { }

  ngOnInit() {
  }

}
