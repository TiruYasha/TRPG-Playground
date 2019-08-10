import { TokenType } from './token-type.enum';

export abstract class Token {
    private _id: string;
    private _x: number;
    private _y: number;
    private _type: TokenType;

    constructor(x: number, y: number, type: TokenType) {
        this._x = x;
        this._y = y;
        this._type = type;
    }

    get x(): number { return this._x; }

    get y(): number { return this._y; }

    get type(): TokenType { return this._type; }
}
