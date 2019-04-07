import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { ActiveGameService } from './active-game.service';
import { SendMessageModel } from 'src/app/models/chat/requests/send-message.model';
import { ReceiveMessageModel } from 'src/app/models/chat/receives/receive-message.model';
import { CommandResult } from 'src/app/models/chat/receives/command-results/command-result.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  receivedMessage = new Subject();

  private connectionIsEstablished = false;
  private _hubConnection: HubConnection;

  constructor(private http: HttpClient, private activeGameService: ActiveGameService) { }

  setup() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  sendMessage(chatMessage: string) {
    if (chatMessage !== '') {

      const message: SendMessageModel = {
        customUsername: '',
        gameId: this.activeGameService.gameId,
        message: chatMessage
      };

      this._hubConnection.invoke('SendMessageToGroup', message);
    }
  }

  addToGroup(): any {
    this._hubConnection.invoke('AddToGroup', this.activeGameService.gameId);
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.apiUrl + '/chathub', { accessTokenFactory: this.getAccessToken })
      .build();
  }

  getAccessToken() {
    const accessToken = localStorage.getItem('token');
    return accessToken;
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.addToGroup();
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...', err);
        setTimeout(this.startConnection, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on('ReceiveMessage', (data: CommandResult) => {
      console.log('received message: ', data);
      this.receivedMessage.next(data);
    });
  }

  getAllMessagesForGame(gameId: string) {
    return this.http.get<ReceiveMessageModel[]>(environment.apiUrl + '/chat/all?gameId=' + gameId);
  }
}
