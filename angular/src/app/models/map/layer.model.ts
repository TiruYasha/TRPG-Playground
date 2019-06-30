import { LayerType } from './layer-type.enum';

export class Layer {
    id: string;
    name: string;
    type: LayerType;
    mapId: string;
    parentId?: string;

    constructor() {
        this.type = LayerType.Default;
    }
}
