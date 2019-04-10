import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { SendMessageModel } from 'src/app/models/chat/requests/send-message.model';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';
import { CommandResult } from 'src/app/models/chat/receives/command-results/command-result.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  receivedMessage = new Subject();

  private hubConnection: HubConnection;

  constructor(private http: HttpClient) { }

  setup(gameId: string) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection(gameId);
  }

  sendMessage(chatMessage: SendMessageModel) {
    this.hubConnection.invoke('SendMessageToGroup', chatMessage);
  }

  addToGroup(gameId: string): any {
    this.hubConnection.invoke('AddToGroup', gameId);
  }

  private createConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.apiUrl + '/chathub', { accessTokenFactory: this.getAccessToken })
      .build();
  }

  getAccessToken() {
    const accessToken = localStorage.getItem('token');
    return accessToken;
  }

  private startConnection(gameId: string): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
        this.addToGroup(gameId);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...', err);
        setTimeout(this.startConnection, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('ReceiveMessage', (data: CommandResult) => {
      console.log('received message: ', data);
      this.receivedMessage.next(data);
    });
  }

  getAllMessagesForGame(gameId: string) {
    return this.http.get<ReceiveMessageModel[]>(environment.apiUrl + '/chat/all?gameId=' + gameId);
  }
}
