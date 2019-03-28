import { Component, OnInit, EventEmitter, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ChatMessage } from '../models/chatmessage.model';
import { ActiveGameService } from '../../active-game.service';
import { ChatService } from '../chat.service';

@Component({
  selector: 'app-chat-window',
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.css']
})
export class ChatWindowComponent implements OnInit, AfterViewChecked {
  @ViewChild('scrollMe') private myScrollContainer: ElementRef;

  messages: ChatMessage[] = [];
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

    this.chatService.receivedMessage.subscribe((data: ChatMessage) => {
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
