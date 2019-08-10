import { DefaultTokenDto } from './default-token.dto';
import { DefaultToken } from '../default-token.model';
import { TokenType } from '../token-type.enum';
import { CharacterTokenDto } from './character-token.dto';

export abstract class DtoTokenFactory {
    static create(token: DefaultToken): DefaultTokenDto {
        let dto: DefaultTokenDto;
        switch (token.type) {
            case TokenType.Character: dto = new CharacterTokenDto();
        }

        dto.mapFromToken(token);
        return dto;
    }
}
