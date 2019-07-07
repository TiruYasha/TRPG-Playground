import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MapManagerComponent } from './map-manager.component';
import { MatIconModule, MatButtonModule, MatDividerModule, MatListModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatMenuModule, MatExpansionModule } from '@angular/material';
import { MapListComponent } from './map-list/map-list.component';
import { LayerManagerComponent } from './layer-manager/layer-manager.component';
import { CreateMapDialogComponent } from './create-map-dialog/create-map-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MapListItemComponent } from './map-list-item/map-list-item.component';
import { LayerListComponent } from './layer-list/layer-list.component';
import { LayerListItemComponent } from './layer-list-item/layer-list-item.component';
import { LayerGroupListItemComponent } from './layer-group-list-item/layer-group-list-item.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [MapManagerComponent, MapListComponent, LayerManagerComponent, CreateMapDialogComponent, MapListItemComponent, LayerListComponent, LayerListItemComponent, LayerGroupListItemComponent],
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
    MatMenuModule,
    MatExpansionModule,
    NoopAnimationsModule
  ],
  exports: [
    MapManagerComponent
  ],
  entryComponents: [CreateMapDialogComponent]
})
export class MapManagerModule { }
