import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Layer } from 'src/app/models/map/layer.model';
import { LayerType } from 'src/app/models/map/layer-type.enum';
import { LayerGroup } from 'src/app/models/map/layer-group.model';

@Component({
  selector: 'trpg-layer-manager',
  templateUrl: './layer-manager.component.html',
  styleUrls: ['./layer-manager.component.scss']
})
export class LayerManagerComponent implements OnInit {

  @Input() layers: Layer[];

  @Output() addLayer = new EventEmitter<Layer>();
  @Output() updateLayer = new EventEmitter<Layer>();
  @Output() delete = new EventEmitter<Layer>();

  editLayer: Layer;

  layerType = LayerType;

  constructor() { }

  ngOnInit() {
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
}
