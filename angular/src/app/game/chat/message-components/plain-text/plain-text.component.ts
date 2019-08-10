import { Component, Input } from '@angular/core';
import { ReceiveMessageModel } from 'src/app/shared/models/chat/receives/receive-message.model';

@Component({
  selector: 'trpg-plain-text',
  templateUrl: './plain-text.component.html',
  styleUrls: ['./plain-text.component.scss']
})
export class PlainTextComponent {
  @Input() chatMessage: ReceiveMessageModel;

  constructor() { }
}
