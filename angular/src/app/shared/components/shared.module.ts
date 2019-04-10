import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SpeedDialFabComponent } from './speed-dial-fab/speed-dial-fab.component';
import { MatIconModule, MatButtonModule } from '@angular/material';

@NgModule({
  declarations: [SpeedDialFabComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule
  ],
  exports: [
    SpeedDialFabComponent
  ]
})
export class SharedModule { }
