import { Token } from './token.model';
import { JournalCharacterSheet } from '../journal/journalitems/journal-character-sheet.model';

export class CharacterToken extends Token {
    characterSheetId: string;
    characterSheet: JournalCharacterSheet;
}
