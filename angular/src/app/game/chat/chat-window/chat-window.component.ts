import { Component, OnInit, EventEmitter, ViewChild, ElementRef, AfterViewChecked, Input } from '@angular/core';
import { ChatService } from '../chat.service';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';
import { SendMessageModel } from 'src/app/models/chat/requests/send-message.model';

@Component({
  selector: 'trpg-chat-window',
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.scss']
})
export class ChatWindowComponent implements OnInit, AfterViewChecked {
  @ViewChild('scrollMe') private myScrollContainer: ElementRef;
  @Input() gameId: string;

  messages: ReceiveMessageModel[] = [];
  chatMessage = '';
  scrolledToBottom = false;

  constructor(private chatService: ChatService) {
  }

  ngOnInit() {
    this.chatService.setup(this.gameId);
    this.chatService.getAllMessagesForGame(this.gameId).subscribe(data => {
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
    if (this.chatMessage === '') {
      return;
    }

    const message: SendMessageModel = {
      customUsername: '',
      gameId: this.gameId,
      message: this.chatMessage
    };

    this.chatService.sendMessage(message);
    this.chatMessage = '';
  }
}
