import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Canvas } from 'fabric/fabric-impl';
import { fabric } from 'fabric';

@Component({
  selector: 'trpg-play-area',
  templateUrl: './play-area.component.html',
  styleUrls: ['./play-area.component.scss']
})
export class PlayAreaComponent implements OnInit, AfterViewInit {

  canvas: Canvas;

  constructor() { }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this.canvas = new fabric.Canvas('c');

    fabric.Image.fromURL('/assets/images/Canvas_test_map.jpg', (oImg) => {
      this.canvas.setWidth(oImg.width);
      this.canvas.setHeight(oImg.height);
      this.canvas.add(oImg);

      const rect = new fabric.Rect({
        left: 100,
        top: 100,
        fill: 'red',
        width: 20,
        height: 20
      });

      this.canvas.add(rect);

      this.draw_grid(200);
    });
  }

  private draw_grid(gridSizeInPixels: number) {

    const width = this.canvas.getWidth();
    const height = this.canvas.getHeight();

    for (let j = 0; j < height; j += gridSizeInPixels) {
      console.log('test');
      const horizontalLine = new fabric.Line([0, j + gridSizeInPixels, width, j + gridSizeInPixels], {
        selectable: false,
        stroke: 'black',
        strokeWidth: 4,
      });

      this.canvas.add(horizontalLine);
    }

    for (let i = 0; i < width; i += gridSizeInPixels) {
      const verticalLine = new fabric.Line([i + gridSizeInPixels, 0, i + gridSizeInPixels, height], {
        selectable: false,
        evented: false,
        stroke: 'black',
        strokeWidth: 4,
      });

      this.canvas.add(verticalLine);
    }
  }
}
