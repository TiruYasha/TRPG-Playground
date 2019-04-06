import { CommandResult } from './command-result.model';
import { CommandType } from './command-type.enum';


export class DefaultCommandResult extends CommandResult {
    constructor() {
        super();
        this.type = CommandType.Default;
    }
}
