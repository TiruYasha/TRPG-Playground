import { Sprite } from 'pixi.js';
import { environment } from 'src/environments/environment';
import { DefaultToken } from '../models/play-area/default-token.model';

export abstract class SpriteFactory {
    static create(token: DefaultToken, order: number): Sprite {
        const sprite = Sprite.from(`${environment.apiUrl}/journal/${token.id}/image`);
        sprite.x = token.x;
        sprite.y = token.y;
        sprite.zIndex = order;

        return sprite;
    }
}
