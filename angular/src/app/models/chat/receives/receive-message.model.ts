import { CommandResult } from './command-results/command-result.model';

export class ReceiveMessageModel {
    constructor(public message: string, public username: string, public commandResult: CommandResult) { }
}
