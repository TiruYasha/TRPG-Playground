import { Component, OnInit, Input } from '@angular/core';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';

@Component({
  selector: 'trpg-error-message',
  templateUrl: './error-message.component.html',
  styleUrls: ['./error-message.component.scss']
})
export class ErrorMessageComponent implements OnInit {
  @Input() chatMessage: ReceiveMessageModel;
  constructor() { }

  ngOnInit() {
  }

}
