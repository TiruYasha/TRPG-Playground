import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalComponent } from './journal.component';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from 'src/app/shared/components/shared.module';
import {
  MatFormFieldModule,
  MatDialogModule,
  MatInputModule,
  MatMenuModule,
  MatAutocompleteModule,
  MatSelectModule,
  MatProgressBarModule,
} from '@angular/material';
import { CreateFolderDialogComponent } from './create-folder-dialog/create-folder-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JournalNodeComponent } from './journal-node/journal-node.component';
import { CreateHandoutDialogComponent } from './create-handout-dialog/create-handout-dialog.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ParentDialogComponent } from './parent-dialog/parent-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    MatTreeModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    SharedModule,
    MatAutocompleteModule,
    DragDropModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatProgressBarModule
  ],
  exports: [
    JournalComponent
  ],
  declarations: [JournalComponent, CreateFolderDialogComponent, JournalNodeComponent, CreateHandoutDialogComponent, ParentDialogComponent],
  entryComponents: [ParentDialogComponent]
})
export class JournalModule { }
