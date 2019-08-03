import { LayerType } from './layer-type.enum';

export class Layer {
    id: string;
    name: string;
    order: number;
    type: LayerType;
    mapId: string;

    constructor() {
        this.type = LayerType.Default;
    }
}
