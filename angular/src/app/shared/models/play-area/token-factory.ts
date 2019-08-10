import { JournalCharacterSheet } from '../journal/journalitems/journal-character-sheet.model';
import { CharacterToken } from './character-token.model';

export abstract class TokenFactory {
    static create(x: number, y: number, journalItem: JournalCharacterSheet) {
        return new CharacterToken(x, y, journalItem.id);
    }
}
