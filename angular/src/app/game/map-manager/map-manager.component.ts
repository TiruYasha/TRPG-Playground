import { Component, OnInit } from '@angular/core';
import { MapService } from '../services/map.service';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { PlayMap } from 'src/app/models/map/map.model';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'trpg-map-manager',
  templateUrl: './map-manager.component.html',
  styleUrls: ['./map-manager.component.scss']
})
export class MapManagerComponent extends DestroySubscription implements OnInit {

  maps: PlayMap[] = [];

  constructor(private mapService: MapService) {
    super();
  }

  ngOnInit() {
    this.mapService.getAllMaps()
      .pipe(takeUntil(this.destroy))
      .subscribe((maps) => {
        this.maps = maps;
      });
  }

  addMap() {
    this.mapService.addMap({
      name: 'test',
      gridSizeInPixels: 100,
      heigthInPixels: 1000,
      widthInPixels: 1000
    })
      .pipe(takeUntil(this.destroy))
      .subscribe((map) => {
        this.maps.push(map);
      });
  }
}
