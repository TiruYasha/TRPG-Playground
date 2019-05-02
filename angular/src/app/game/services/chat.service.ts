import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';
import { ActiveGameService } from './active-game.service';
import { SendMessageModel } from 'src/app/models/chat/requests/send-message.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private chatMessageSentSubject = new Subject<ReceiveMessageModel>();

  chatMessageSent = this.chatMessageSentSubject.asObservable();

  constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

  setup() {
    this.registerOnServerEvents();
  }

  sendMessage(chatMessage: SendMessageModel) {
    return this.http.post(environment.apiUrl + '/chat/sendMessage', chatMessage);
  }

  getAllMessagesForGame() {
    return this.http.get<ReceiveMessageModel[]>(environment.apiUrl + '/chat/all');
  }

  private registerOnServerEvents() {
    this.activeGameService.hubConnection.on('ChatMessageSent', (data: ReceiveMessageModel) => {
      this.chatMessageSentSubject.next(data);
    });
  }
}
