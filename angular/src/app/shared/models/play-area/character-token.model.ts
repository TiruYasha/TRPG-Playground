import { DefaultToken } from './default-token.model';
import { TokenType } from './token-type.enum';

export class CharacterToken extends DefaultToken {
    characterSheetId: string;

    constructor(x: number, y: number, characterSheetId: string) {
        super(x, y, TokenType.Character);
        this.characterSheetId = characterSheetId;
    }
}
