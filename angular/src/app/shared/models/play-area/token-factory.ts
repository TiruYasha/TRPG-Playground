import { JournalCharacterSheet } from '../journal/journalitems/journal-character-sheet.model';
import { CharacterToken } from './character-token.model';
import { DefaultTokenDto } from './backend/default-token.dto';
import { DefaultToken } from './default-token.model';
import { CharacterTokenDto } from './backend/character-token.dto';

export abstract class TokenFactory {
    static create(x: number, y: number, journalItem: JournalCharacterSheet) {
        const dto = new CharacterTokenDto();
        dto.characterSheetId = journalItem.id;
        dto.x = x;
        dto.y = y;
        const token = new CharacterToken(dto as CharacterTokenDto);
        return token;
    }

    static createFromDto(dto: DefaultTokenDto): DefaultToken {
        const token = new CharacterToken(dto as CharacterTokenDto);
        return token;
    }
}
