import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayAreaComponent } from './play-area.component';

@NgModule({
  declarations: [PlayAreaComponent],
  imports: [
    CommonModule
  ],
  exports: [
    PlayAreaComponent
  ],
  entryComponents: [PlayAreaComponent]
})
export class PlayAreaModule { }
