import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CreateFolderDialogModel } from './create-folder-dialog.model';
import { ParentDialogComponent } from '../parent-dialog/parent-dialog.component';
import { FormControl, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/utilities/CustomValidators';

@Component({
  selector: 'trpg-create-folder-dialog',
  templateUrl: './create-folder-dialog.component.html'
})
export class CreateFolderDialogComponent implements OnInit {
  name = new FormControl('', [Validators.required, CustomValidators.noWhitespaceValidator]);

  constructor(
    public dialogRef: MatDialogRef<ParentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateFolderDialogModel) { }

  ngOnInit(): void {
    this.name.setValue(this.data.name);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  Ok() {
    this.data.name = this.name.value;
    this.dialogRef.close(this.data.name);
  }
}
