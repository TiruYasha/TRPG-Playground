import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapManagerComponent } from './map-manager.component';
import { MatIconModule, MatButtonModule, MatDividerModule, MatListModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatMenuModule } from '@angular/material';
import { MapListComponent } from './map-list/map-list.component';
import { LayerManagerComponent } from './layer-manager/layer-manager.component';
import { CreateMapDialogComponent } from './create-map-dialog/create-map-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [MapManagerComponent, MapListComponent, LayerManagerComponent, CreateMapDialogComponent],
  imports: [
    CommonModule,
    MatIconModule,
    MatButtonModule,
    MatDividerModule,
    MatListModule,
    MatDialogModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatMenuModule
  ],
  exports: [
    MapManagerComponent
  ],
  entryComponents: [CreateMapDialogComponent]
})
export class MapManagerModule { }
