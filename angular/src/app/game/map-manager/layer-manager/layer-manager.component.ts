import { Component, OnInit, Input } from '@angular/core';
import { Layer } from 'src/app/models/map/layer.model';
import { LayerType } from 'src/app/models/map/layer-type.enum';
import { LayerGroup } from 'src/app/models/map/layer-group.model';
import { MapService } from '../../services/map.service';
import { PlayMap } from 'src/app/models/map/map.model';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { takeUntil } from 'rxjs/operators';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ChangeOrder } from 'src/app/models/map/requests/change-order.model';

@Component({
  selector: 'trpg-layer-manager',
  templateUrl: './layer-manager.component.html',
  styleUrls: ['./layer-manager.component.scss']
})
export class LayerManagerComponent extends DestroySubscription implements OnInit {

  @Input() layers: Layer[] = [];

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

    const lastLayer = this.layers[this.layers.length - 1];

    if (lastLayer) {
      layer.order = lastLayer.order + 1;
    } else {
      layer.order = 0;
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

  drop(event: CdkDragDrop<Layer[]>) {
    const layerToMove = this.layers[event.previousIndex];
    const newOrder = this.layers[event.currentIndex].order;

    const changeOrder: ChangeOrder = {
      PreviousPosition: layerToMove.order,
      NewPosition: newOrder
    };

    if (changeOrder.PreviousPosition > changeOrder.NewPosition) {
      this.layers.slice(event.currentIndex, event.previousIndex).forEach(l => {
        l.order += 1;
      });
    } else {
      this.layers.slice(event.previousIndex + 1, event.currentIndex + 1).forEach(l => {
        l.order -= 1;
      });
    }

    layerToMove.order = newOrder;

    this.mapService.updateLayerOrder(changeOrder, this.map.id, layerToMove.id)
      .pipe(takeUntil(this.destroy))
      .subscribe(() => { });

    moveItemInArray(this.layers, event.previousIndex, event.currentIndex);
  }
}
