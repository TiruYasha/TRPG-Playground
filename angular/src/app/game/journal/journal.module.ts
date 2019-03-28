import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JournalComponent } from './journal.component';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  imports: [
    CommonModule,
    MatTreeModule,
    MatIconModule
  ],
  exports: [
    JournalComponent
  ],
  declarations: [JournalComponent]
})
export class JournalModule { }
