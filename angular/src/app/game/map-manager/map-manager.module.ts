import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapManagerComponent } from './map-manager.component';
import { MatIconModule, MatButtonModule, MatDividerModule, MatListModule } from '@angular/material';
import { MapListComponent } from './map-list/map-list.component';
import { LayerManagerComponent } from './layer-manager/layer-manager.component';

@NgModule({
  declarations: [MapManagerComponent, MapListComponent, LayerManagerComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatDividerModule,
    MatListModule
  ],
  exports: [
    MapManagerComponent
  ]
})
export class MapManagerModule { }
