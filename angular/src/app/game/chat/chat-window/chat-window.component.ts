import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';
import { SendMessageModel } from 'src/app/models/chat/requests/send-message.model';
import { ChatService } from '../../services/chat.service';
import { ActiveGameService } from '../../services/active-game.service';

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

  constructor(private chatService: ChatService, private activeGameService: ActiveGameService) {
  }

  ngOnInit() {
    this.chatService.setup();
    this.chatService.getAllMessagesForGame().subscribe(data => {
      this.messages = data;
    });

    this.chatService.chatMessageSent.subscribe((data: ReceiveMessageModel) => {
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
      gameId: this.activeGameService.activeGameId,
      message: this.chatMessage
    };

    this.chatService.sendMessage(message)
      .subscribe(() => {

      });
    this.chatMessage = '';
  }
}
