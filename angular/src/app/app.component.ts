import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { RegisterModel } from './models/login.model';
import { environment } from 'src/environments/environment';
import { LoginSuccessModel } from './models/loginsuccess.model';
import { GameService } from './shared/game.service';
import { Game } from './models/game.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'dnd';

  constructor(private http: HttpClient, private gameService: GameService, private router: Router) {

  }

  ngOnInit(): void {}
}
