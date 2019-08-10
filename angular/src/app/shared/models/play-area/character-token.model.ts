import { Token } from './token.model';
import { TokenType } from './token-type.enum';

export class CharacterToken extends Token {
    characterSheetId: string;

    constructor(x: number, y: number) {
        super(x, y, TokenType.Character);
    }
}
