import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Application, Sprite, Graphics, interaction } from 'pixi.js';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { takeUntil } from 'rxjs/operators';
import { GameStateService } from '../../shared/services/game-state.service';
import { DragService } from '../../shared/services/drag.service';
import { environment } from 'src/environments/environment';
import { LayerService } from '../../shared/services/layer.service';
import { CharacterToken } from 'src/app/shared/models/play-area/character-token.model';
import { PlayMap } from 'src/app/shared/models/map/map.model';

@Component({
  selector: 'trpg-play-area',
  templateUrl: './play-area.component.html',
  styleUrls: ['./play-area.component.scss']
})
export class PlayAreaComponent extends DestroySubscription implements OnInit {
  private isOwner = false;

  @ViewChild('canvasContainer') canvasContainer: ElementRef;
  application: Application;

  constructor(private gameState: GameStateService,
    private layerSerivce: LayerService,
    private dragService: DragService) { super(); }

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

  dragOver(event: DragEvent) {
    event.preventDefault();
  }

  drop(event: DragEvent) {
    event.preventDefault();
    console.log(event);
    const item = this.dragService.itemBeingDragged;
    let sprite = Sprite.from(`${environment.apiUrl}/journal/${item.id}/image`);
    sprite.x = event.layerX;
    sprite.y = event.layerY;
    sprite.zIndex = 200;
    this.application.stage.addChild(sprite);
  }

  private changeMap(map: PlayMap): void {
    this.application.stage.removeChildren();
    const canvasWidth = map.widthInPixels;
    const canvasHeight = map.heightInPixels;

    this.application.renderer.resize(canvasWidth, canvasHeight);
    this.draw_grid(map.gridSizeInPixels);
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
