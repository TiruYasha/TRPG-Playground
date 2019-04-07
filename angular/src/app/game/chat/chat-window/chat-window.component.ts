import { Component, OnInit, EventEmitter, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ChatService } from '../chat.service';
import { ActiveGameService } from '../../services/active-game.service';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';

@Component({
  selector: 'trpg-chat-window',
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.scss']
})
export class ChatWindowComponent implements OnInit, AfterViewChecked {
  @ViewChild('scrollMe') private myScrollContainer: ElementRef;

  messages: ReceiveMessageModel[] = [];
  chatMessage = '';
  scrolledToBottom = false;

  constructor(private activeGameService: ActiveGameService, private chatService: ChatService) {
  }

  ngOnInit() {
    this.chatService.setup();
    this.chatService.getAllMessagesForGame(this.activeGameService.gameId).subscribe(data => {
      console.log(data);
      this.messages = data;
    });

    this.chatService.receivedMessage.subscribe((data: ReceiveMessageModel) => {
      this.checkScrollAtBottom();

      this.messages.push(data);
    });
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private checkScrollAtBottom() {
    const scrollElement = this.myScrollContainer.nativeElement;
    if (scrollElement.scrollTop === (scrollElement.scrollHeight - scrollElement.offsetHeight)) {
      this.scrolledToBottom = false;
    }
  }

  scrollToBottom() {
    const scrollElement = this.myScrollContainer.nativeElement;

    if (this.scrolledToBottom === false && this.messages.length > 1) {

      scrollElement.scrollTop = scrollElement.scrollHeight;
      this.scrolledToBottom = true;
    }
  }

  sendMessage() {
    this.chatService.sendMessage(this.chatMessage);
    this.chatMessage = '';
  }
}
