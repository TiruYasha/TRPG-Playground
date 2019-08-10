import { DefaultToken } from './default-token.model';
import { CharacterTokenDto } from './backend/character-token.dto';
import { TokenType } from './token-type.enum';

export class CharacterToken extends DefaultToken {
    private _characterSheetId: string;
    constructor(dto: CharacterTokenDto) {
        super(dto, TokenType.Character);
        this._characterSheetId = dto.characterSheetId;
    }

    get characterSheetId(): string { return this._characterSheetId; }
}
