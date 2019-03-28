import { CommandResult } from './command/command-result.model';

export class ChatMessage {

  user: string;
  message: string;
  commandResult: CommandResult;

  constructor(user: string = '', message: string = '') {
    this.user = user;
    this.message = message;
  }
}
