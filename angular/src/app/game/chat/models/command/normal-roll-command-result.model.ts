import { CommandResult } from './command-result.model';

export class NormalRollCommandResult implements CommandResult {
    type: number;
    result: string;
}
