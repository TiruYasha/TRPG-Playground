import { Component, OnInit, Input} from '@angular/core';
import { ChatMessage } from '../../models/chatmessage.model';

@Component({
  selector: 'app-normal-roll-result',
  templateUrl: './normal-roll-result.component.html',
  styleUrls: ['./normal-roll-result.component.css']
})
export class NormalRollResultComponent implements OnInit {
  @Input() chatMessage: ChatMessage;

  constructor() { }

  ngOnInit() {
  }
}
