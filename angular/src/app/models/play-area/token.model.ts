import { TokenType } from './token-type.enum';

export abstract class Token {
    id: string;
    x: number;
    y: number;
    type: TokenType;
}
