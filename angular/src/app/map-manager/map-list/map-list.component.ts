import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { takeUntil } from 'rxjs/operators';
import { CreateMapDialogModel } from '../create-map-dialog/create-map-dialog.model';
import { CreateMapDialogComponent } from '../create-map-dialog/create-map-dialog.component';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { PlayMap } from 'src/app/shared/models/map/map.model';
import { DialogState } from 'src/app/shared/models/dialog-state.enum';
import { GameStateService } from 'src/app/shared/services/game-state.service';
import { MapService } from 'src/app/shared/services/map.service';

@Component({
  selector: 'trpg-map-list',
  templateUrl: './map-list.component.html',
  styleUrls: ['./map-list.component.scss']
})
export class MapListComponent extends DestroySubscription implements OnInit {
  maps: PlayMap[] = [];
  selectedMap: PlayMap;
  visibleMap: string;

  constructor(private gameState: GameStateService, private mapService: MapService, private dialog: MatDialog) {
    super();
  }

  ngOnInit() {
    this.mapService.getAllMaps()
      .pipe(takeUntil(this.destroy))
      .subscribe((maps) => {
        this.maps = maps;
      });

    this.gameState.mapVisibilityChangedObservable
      .pipe(takeUntil(this.destroy))
      .subscribe((map) => {
        this.visibleMap = map.id;
      });
  }

  changeMap(map: PlayMap) {
    this.selectedMap = map;
    this.gameState.selectMap(map);
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

  makeMapVisible(map: PlayMap) {
    this.mapService.makeMapVisible(map.id)
      .pipe(takeUntil(this.destroy))
      .subscribe(() => {
        this.gameState.changeVisibleMap(map);
      });
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
