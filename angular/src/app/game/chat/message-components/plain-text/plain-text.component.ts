import { Component, OnInit, Input } from '@angular/core';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';

@Component({
  selector: 'trpg-plain-text',
  templateUrl: './plain-text.component.html',
  styleUrls: ['./plain-text.component.css']
})
export class PlainTextComponent {
  @Input() chatMessage: ReceiveMessageModel;

  constructor() { }
}
