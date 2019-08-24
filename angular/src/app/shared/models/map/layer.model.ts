import { LayerType } from './layer-type.enum';
import { DefaultToken } from '../play-area/default-token.model';

export class Layer {
    id: string;
    name: string;
    order: number;
    type: LayerType;
    isVisible: boolean;
    isVisibleToPlayers: boolean;
    mapId: string;
    tokens: DefaultToken[];

    constructor() {
        this.type = LayerType.Default;
        this.tokens = [];
    }
}
