import { Component, OnInit } from '@angular/core';
import { MapService } from '../services/map.service';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { PlayMap } from 'src/app/models/map/map.model';
import { takeUntil } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { CreateMapDialogComponent } from './create-map-dialog/create-map-dialog.component';
import { CreateMapDialogModel } from './create-map-dialog/create-map-dialog.model';
import { DialogState } from 'src/app/models/dialog-state.enum';

@Component({
  selector: 'trpg-map-manager',
  templateUrl: './map-manager.component.html',
  styleUrls: ['./map-manager.component.scss']
})
export class MapManagerComponent extends DestroySubscription implements OnInit {

  maps: PlayMap[] = [];

  constructor(private mapService: MapService, private dialog: MatDialog) {
    super();
  }

  ngOnInit() {
    this.mapService.getAllMaps()
      .pipe(takeUntil(this.destroy))
      .subscribe((maps) => {
        this.maps = maps;
      });
  }

  changeMap(map: PlayMap) {
    this.mapService.changeMap(map);
  }

  addMap() {
    this.openMapDialog(DialogState.New);
  }

  deleteMap(map: PlayMap) {
    this.mapService.deleteMap(map.id)
      .pipe(takeUntil(this.destroy))
      .subscribe(() => {
        this.maps = this.maps.filter(m => m.id !== map.id);
      });
  }

  editMap(map: PlayMap) {
    this.openMapDialog(DialogState.Edit, map);
  }

  private openMapDialog(dialogState: DialogState, map: PlayMap = null) {
    const data: CreateMapDialogModel = {
      dialogState: dialogState,
      map: map
    };
    const dialogRef = this.dialog.open(CreateMapDialogComponent, {
      width: 'auto',
      data: data,
      hasBackdrop: false
    });
    dialogRef.afterClosed().pipe(takeUntil(this.destroy))
      .subscribe((map: PlayMap) => {
        if (!map) { return; }

        if (dialogState === DialogState.New) {
          this.maps.push(map);
        } else if (dialogState === DialogState.Edit) {
          const index = this.maps.findIndex(m => m.id === map.id);
          this.maps[index] = map;
        }
      });
  }
}
