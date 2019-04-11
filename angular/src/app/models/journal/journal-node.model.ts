import { JournalItem } from 'src/app/models/journal/journalitems/journal-item.model';
export interface JournalNode {
  expandable: boolean;
  item: JournalItem;
  level: number;
}
