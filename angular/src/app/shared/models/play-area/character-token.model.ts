import { DefaultToken } from './default-token.model';
import { CharacterTokenDto } from './backend/character-token.dto';
import { TokenType } from './token-type.enum';
import { Sprite, DisplayObject } from 'pixi.js';
import { environment } from 'src/environments/environment';

export class CharacterToken extends DefaultToken {
    private _characterSheetId: string;
    constructor(dto: CharacterTokenDto) {
        super(dto, TokenType.Character);
        this._characterSheetId = dto.characterSheetId;
    }

    get characterSheetId(): string { return this._characterSheetId; }

    createToken(order: number): DisplayObject {
        const sprite = Sprite.from(`${environment.apiUrl}/journal/${this._characterSheetId}/token`);
        sprite.x = this.x;
        sprite.y = this.y;
        sprite.zIndex = order;

        return sprite;
    }
}
