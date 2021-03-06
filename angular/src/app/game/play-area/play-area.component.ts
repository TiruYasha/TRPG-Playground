import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';

import { Application, utils, Sprite, Loader, Graphics, interaction } from 'pixi.js';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { MapService } from '../services/map.service';
import { takeUntil } from 'rxjs/operators';
import { PlayMap } from 'src/app/models/map/map.model';
import { GameStateService } from '../services/game-state.service';

@Component({
  selector: 'trpg-play-area',
  templateUrl: './play-area.component.html',
  styleUrls: ['./play-area.component.scss']
})
export class PlayAreaComponent extends DestroySubscription implements OnInit, AfterViewInit {
  private isOwner = false;

  @ViewChild('canvasContainer') canvasContainer: ElementRef;
  application: Application;

  constructor(private gameState: GameStateService, private mapService: MapService) { super(); }

  ngOnInit() {
    const div = this.canvasContainer.nativeElement as HTMLDivElement;
    this.application = new Application(
    );
    this.application.renderer.backgroundColor = 0xFFFFFF;
    this.application.stage.sortableChildren = true;

    div.appendChild(this.application.view);

    this.gameState.isOwnerObservable.pipe(takeUntil(this.destroy))
      .subscribe(isOwner => {
        this.isOwner = isOwner;
        if (this.isOwner) {
          this.gameState.changeMapObservable.pipe(takeUntil(this.destroy))
            .subscribe(map => this.changeMap(map));
        } else {
          this.gameState.mapVisibilityChangedObservable.pipe(takeUntil(this.destroy))
            .subscribe(map => this.changeMap(map));
        }
      });
  }

  private changeMap(map: PlayMap): void {
    this.application.stage.removeChildren();
    const canvasWidth = map.widthInPixels;
    const canvasHeight = map.heightInPixels;

    this.application.renderer.resize(canvasWidth, canvasHeight);
    this.draw_grid(map.gridSizeInPixels);
  }

  ngAfterViewInit(): void {
    // const div = this.canvasContainer.nativeElement as HTMLDivElement;
    // this.application = new Application(
    // );
    // this.application.renderer.backgroundColor = 0xFFFFFF;

    // div.appendChild(this.application.view);

    // const testing = new Loader();

    // testing
    //   .add('/assets/images/Canvas_test_map.jpg')
    //   .load(() => {
    //     const background = new Sprite(testing.resources['/assets/images/Canvas_test_map.jpg'].texture);
    //     this.application.stage.sortableChildren = true;
    //     background.zIndex = -1;

    //     const canvasWidth = background.width > 4000 ? 4000 : background.width;
    //     const canvasHeight = background.height > 4000 ? 4000 : background.height;

    //     this.application.renderer.resize(canvasWidth, canvasHeight);
    //     this.application.stage.addChild(background);
    //     this.draw_grid(100);
    //     this.addDraggableRectangle();
    //   });
  }

  private draw_grid(gridSizeInPixels: number) {

    const width = this.application.renderer.width;
    const height = this.application.renderer.height;

    for (let j = 0; j < height; j += gridSizeInPixels) {
      const horizontalLine = new Graphics();
      horizontalLine.lineStyle(1, 0x00000, 1);
      horizontalLine.moveTo(0, j);
      horizontalLine.lineTo(width, j);
      horizontalLine.x = 0;
      horizontalLine.y = j;
      horizontalLine.zIndex = 100;
      this.application.stage.addChild(horizontalLine);
    }

    for (let i = 0; i < width; i += gridSizeInPixels) {
      const verticalLine = new Graphics();
      verticalLine.lineStyle(1, 0x00000, 1);
      verticalLine.moveTo(i, 0);
      verticalLine.lineTo(i, height);
      verticalLine.x = i;
      verticalLine.y = 0;
      verticalLine.zIndex = 100;
      this.application.stage.addChild(verticalLine);
    }
  }

  rectangle: Graphics;

  private addDraggableRectangle() {
    this.rectangle = new Graphics();
    this.rectangle.beginFill(0x66CCFF);
    this.rectangle.zIndex = 10;
    this.rectangle.drawRect(0, 0, 50, 50);
    this.rectangle.endFill();
    this.rectangle.interactive = true;
    this.rectangle.buttonMode = true;
    this.rectangle
      .on('pointerdown', (event) => this.onDragStart(event))
      .on('pointerup', () => this.onDragEnd())
      .on('pointerupoutside', () => this.onDragEnd())
      .on('pointermove', () => this.onDragMove());

    this.application.stage.addChild(this.rectangle);
  }

  data: interaction.InteractionData;
  alpha = 0.5;
  dragging = false;

  onDragStart(event: interaction.InteractionEvent) {
    // store a reference to the data
    // the reason for this is because of multitouch
    // we want to track the movement of this particular touch
    this.data = event.data;
    this.alpha = 0.5;
    this.dragging = true;
  }

  onDragEnd() {
    this.alpha = 1;
    this.dragging = false;
    // set the interaction data to null
    this.data = null;
  }

  onDragMove() {

    if (this.dragging) {
      console.log(this);

      const newPosition = this.data.getLocalPosition(this.rectangle.parent);
      this.rectangle.x = newPosition.x;
      this.rectangle.y = newPosition.y;
    }
  }
}
