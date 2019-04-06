import { CommandResult } from './command-result.model';
import { CommandType } from './command-type.enum';

export class NormalDiceRollCommandResult extends CommandResult {
    public rollResult: number;

    constructor() {
        super();
        this.type = CommandType.NormallDiceRoll;
    }
}
