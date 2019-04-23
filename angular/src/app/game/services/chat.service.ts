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

  // setup() {
  //   this.createConnection();
  //   this.registerOnServerEvents();
  //   this.startConnection();
  // }

  // sendMessage(chatMessage: SendMessageModel) {
  //   this.hubConnection.invoke('SendMessageToGroup', chatMessage);
  // }

  // addToGroup(): any {
  //   this.hubConnection.invoke('AddToGroup', this.activeGameService.activeGameId);
  // }

  // private createConnection() {
  //   this.hubConnection = new HubConnectionBuilder()
  //     .withUrl(environment.apiUrl + '/chathub', { accessTokenFactory: this.getAccessToken })
  //     .build();
  // }

  // getAccessToken() {
  //   const accessToken = localStorage.getItem('token');
  //   return accessToken;
  // }

  // private startConnection(): void {
  //   this.hubConnection
  //     .start()
  //     .then(() => {
  //       console.log('Hub connection started');
  //       this.addToGroup();
  //     })
  //     .catch(err => {
  //       console.log('Error while establishing connection, retrying...', err);
  //       setTimeout(this.startConnection, 5000);
  //     });
  // }

  // private registerOnServerEvents(): void {
  //   this.hubConnection.on('ReceiveMessage', (data: CommandResult) => {
  //     console.log('received message: ', data);
  //     this.receivedMessage.next(data);
  //   });
  // }

 
}
