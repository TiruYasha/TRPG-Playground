import { Component, OnInit, Input } from '@angular/core';
import { Layer } from 'src/app/models/map/layer.model';
import { LayerType } from 'src/app/models/map/layer-type.enum';
import { LayerGroup } from 'src/app/models/map/layer-group.model';
import { MapService } from '../../services/map.service';
import { PlayMap } from 'src/app/models/map/map.model';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'trpg-layer-manager',
  templateUrl: './layer-manager.component.html',
  styleUrls: ['./layer-manager.component.scss']
})
export class LayerManagerComponent extends DestroySubscription implements OnInit {

  @Input() layers: Layer[];

  map: PlayMap;
  editLayer: Layer;

  layerType = LayerType;

  constructor(private mapService: MapService) { super(); }

  ngOnInit() {
    this.mapService.changeMapObservable
      .pipe(takeUntil(this.destroy))
      .subscribe(map => {
        this.map = map;
        this.mapService.getLayers(map.id)
          .pipe(takeUntil(this.destroy))
          .subscribe(layers => {
            this.layers = layers;
          });
      });
  }

  addNewLayer(layerType: LayerType) {
    let layer: Layer;

    switch (layerType) {
      case LayerType.Default:
        layer = new Layer();
        break;
      case LayerType.LayerGroup:
        layer = new LayerGroup();
        break;
    }

    this.layers.push(layer);
    this.editLayer = layer;
  }

  cancelLayerAdd(layer: Layer) {
    this.layers = this.layers.filter(l => l !== layer);
  }

  addLayer(layer: Layer) {
    this.mapService.addLayer(this.map.id, layer)
      .pipe(takeUntil(this.destroy))
      .subscribe(newLayer => {
        if (layer.LayerGroupId) {
          const parent = this.layers.filter(l => l.id === layer.LayerGroupId)[0] as LayerGroup;
          const index = parent.layers.findIndex(l => l === layer);
          parent.layers[index].id = newLayer.id;
        } else {
          const index = this.layers.findIndex(l => l === layer);
          this.layers[index].id = newLayer.id;
        }
      });
  }

  updateLayer(layer: Layer) {
    this.mapService.updateLayer(this.map.id, layer)
      .pipe(takeUntil(this.destroy))
      .subscribe();
  }

  deleteLayer(layer: Layer) {
    this.mapService.deleteLayer(this.map.id, layer.id)
      .pipe(takeUntil(this.destroy))
      .subscribe(() => {
        this.layers = this.layers.filter(l => l !== layer);
      });
  }
}
