import { DefaultTokenDto } from './default-token.dto';
import { CharacterToken } from '../character-token.model';

export class CharacterTokenDto extends DefaultTokenDto {
    characterSheetId: string;

    mapFromToken(token: CharacterToken) {
        super.mapFromToken(token);
        this.characterSheetId = token.characterSheetId;
    }
}
