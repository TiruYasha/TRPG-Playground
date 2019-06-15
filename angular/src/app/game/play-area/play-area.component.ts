import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'trpg-play-area',
  templateUrl: './play-area.component.html',
  styleUrls: ['./play-area.component.scss']
})
export class PlayAreaComponent implements OnInit {

  @ViewChild('canvas') canvas;

  constructor() { }

  ngOnInit() {
    console.log(this.canvas);
  }

}
