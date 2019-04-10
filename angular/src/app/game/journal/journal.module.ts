import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalComponent } from './journal.component';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import { SharedModule } from 'src/app/shared/components/shared.module';
import { MatFormFieldModule, MatDialogModule, MatInputModule } from '@angular/material';
import { CreateFolderDialogComponent } from './create-folder-dialog/create-folder-dialog.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    MatTreeModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    SharedModule
  ],
  exports: [
    JournalComponent
  ],
  declarations: [JournalComponent, CreateFolderDialogComponent],
  entryComponents: [CreateFolderDialogComponent]
})
export class JournalModule { }
