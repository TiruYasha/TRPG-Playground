import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Layer } from 'src/app/models/map/layer.model';

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

  constructor() { }

  ngOnInit() {
  }

  addNewLayer() {
    const layer = new Layer();
    this.layers.push(layer);
    this.editLayer = layer;
  }

  cancelLayerAdd(layer: Layer) {
    this.layers = this.layers.filter(l => l !== layer);
  }
}
