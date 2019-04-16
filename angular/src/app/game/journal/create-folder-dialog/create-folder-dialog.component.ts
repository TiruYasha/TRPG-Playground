import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CreateFolderDialogModel } from './create-folder-dialog.model';
import { ParentDialogComponent } from '../parent-dialog/parent-dialog.component';

@Component({
  selector: 'trpg-create-folder-dialog',
  templateUrl: './create-folder-dialog.component.html'
})
export class CreateFolderDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<ParentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateFolderDialogModel) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}
