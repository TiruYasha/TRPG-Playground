import { Sprite } from 'pixi.js';
import { environment } from 'src/environments/environment';
import { DefaultToken } from '../models/play-area/default-token.model';
import { CharacterToken } from '../models/play-area/character-token.model';

export abstract class SpriteFactory {
    static create(token: CharacterToken, order: number): Sprite {
        const sprite = Sprite.from(`${environment.apiUrl}/journal/${token.characterSheetId}/token`);
        sprite.x = token.x;
        sprite.y = token.y;
        sprite.zIndex = order;

        return sprite;
    }
}
