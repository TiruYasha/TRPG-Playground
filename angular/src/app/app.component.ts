import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GameService } from './shared/game.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  title = 'dnd';

  constructor(private http: HttpClient, private gameService: GameService, private router: Router) {

  }

  ngOnInit(): void {}
}
