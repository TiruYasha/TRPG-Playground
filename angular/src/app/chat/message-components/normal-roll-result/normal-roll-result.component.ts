import { Component, OnInit, Input } from '@angular/core';
import { ReceiveMessageModel } from 'src/app/shared/models/chat/receives/receive-message.model';
import { NormalDiceRollCommandResult } from 'src/app/shared/models/chat/command-results/normal-dice-roll-command-result.model';

@Component({
  selector: 'trpg-normal-roll-result',
  templateUrl: './normal-roll-result.component.html',
  styleUrls: ['./normal-roll-result.component.scss']
})
export class NormalRollResultComponent implements OnInit {
  @Input() chatMessage: ReceiveMessageModel;

  constructor() { }

  ngOnInit() {
  }

  getResult(): number {
    const normalDiceRollCommandResult = this.chatMessage.commandResult as NormalDiceRollCommandResult;

    return normalDiceRollCommandResult.rollResult;
  }
}
