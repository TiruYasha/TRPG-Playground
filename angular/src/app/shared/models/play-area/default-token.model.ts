import { TokenType } from './token-type.enum';
import { DefaultTokenDto } from './backend/default-token.dto';

export abstract class DefaultToken {
    private _id: string;
    private _x: number;
    private _y: number;
    private _type: TokenType;

    constructor(dto: DefaultTokenDto, type: TokenType) {
        this._id = dto.id;
        this._x = dto.x;
        this._y = dto.y;
        this._type = type;
    }

    get id(): string { return this._id; }
    get x(): number { return this._x; }
    get y(): number { return this._y; }
    get type(): TokenType { return this._type; }
}
