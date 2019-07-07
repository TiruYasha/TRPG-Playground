import { LayerType } from './layer-type.enum';
import { Layer } from './layer.model';

export class LayerGroup extends Layer {
    layers: Layer[];

    constructor() {
        super();
        this.type = LayerType.LayerGroup;
        this.layers = [];
    }
}
