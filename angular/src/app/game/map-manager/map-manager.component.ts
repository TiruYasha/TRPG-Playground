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
    const data: CreateMapDialogModel = {
      dialogState: DialogState.New
    };
    const dialogRef = this.dialog.open(CreateMapDialogComponent, {
      width: 'auto',
      data: data,
      hasBackdrop: false
    });

    dialogRef.afterClosed().pipe(takeUntil(this.destroy))
      .subscribe((map: PlayMap) => {
        this.maps.push(map);
      });
  }
}
