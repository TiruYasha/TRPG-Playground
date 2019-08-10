import { TokenType } from '../token-type.enum';
import { DefaultToken } from '../default-token.model';

export class DefaultTokenDto {
    id: string;
    x: number;
    y: number;
    type: TokenType;

    mapFromToken(defaultToken: DefaultToken): void {
        this.id = defaultToken.id;
        this.x = defaultToken.x;
        this.y = defaultToken.y;
        this.type = defaultToken.type;
    }
}
