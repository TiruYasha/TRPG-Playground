import { Component, OnInit, Input } from '@angular/core';
import { ChatMessage } from '../../models/chatmessage.model';

@Component({
  selector: 'app-plain-text',
  templateUrl: './plain-text.component.html',
  styleUrls: ['./plain-text.component.css']
})
export class PlainTextComponent {
  @Input() chatMessage: ChatMessage;

  constructor() { }
}
