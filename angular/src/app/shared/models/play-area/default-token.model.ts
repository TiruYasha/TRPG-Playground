import { TokenType } from './token-type.enum';
import { DefaultTokenDto } from './backend/default-token.dto';
import { DisplayObject } from 'pixi.js';

export abstract class DefaultToken {

    private _id: string;
    private _x: number;
    private _y: number;
    private _type: TokenType;
    private _sprite: DisplayObject;

    constructor(dto: DefaultTokenDto, type: TokenType) {
        this._id = dto.id;
        this._x = dto.x;
        this._y = dto.y;
        this._type = type;
        this._sprite = this.createToken(0);
        this._sprite.interactive = true;
        this._sprite.buttonMode = true;
        this._sprite
            .on('pointerdown', (event) => this.onDragStart(event))
            .on('pointerup', () => this.onDragEnd())
            .on('pointerupoutside', () => this.onDragEnd())
            .on('pointermove', () => this.onDragMove());
    }

    get id(): string { return this._id; }
    get x(): number { return this._x; }
    get y(): number { return this._y; }
    get type(): TokenType { return this._type; }

    protected abstract createToken(order: number): DisplayObject;

    onDragMove(): void {
        throw new Error("Method not implemented.");
    }
    onDragEnd(): void {
        throw new Error("Method not implemented.");
    }
    onDragStart(event: PIXI.interaction.InteractionEvent): void {
        throw new Error("Method not implemented.");
    }
}
