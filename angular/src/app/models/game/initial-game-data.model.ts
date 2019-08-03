import { Player } from './player.model';
import { PlayMap } from '../map/map.model';

export class InitialGameData {
    id: string;
    name: string;
    isOwner: boolean;
    visibleMap: PlayMap;
    players: Player[] = [];
}
