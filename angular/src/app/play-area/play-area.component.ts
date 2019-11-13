import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Application } from 'pixi.js';
import { DestroySubscription } from 'src/app/shared/components/destroy-subscription.extendable';
import { takeUntil } from 'rxjs/operators';
import { PlayMap } from 'src/app/shared/models/map/map.model';
import { GridDrawer } from 'src/app/shared/utilities/grid-drawer.util';
import { SpriteFactory } from 'src/app/shared/utilities/sprite-factory';
import { TokenFactory } from 'src/app/shared/models/play-area/token-factory';
import { Layer } from 'src/app/shared/models/map/layer.model';
import { MapService } from 'src/app/shared/services/map.service';
import { CharacterToken } from 'src/app/shared/models/play-area/character-token.model';
import { GameStateService } from '../shared/services/game-state.service';
import { LayerService } from '../shared/services/layer.service';
import { DragService } from '../shared/services/drag.service';
import { ToolboxOption } from '../shared/models/play-area/toolbox-option.enum';

@Component({
  selector: 'trpg-play-area',
  templateUrl: './play-area.component.html',
  styleUrls: ['./play-area.component.scss']
})
export class PlayAreaComponent extends DestroySubscription implements OnInit {
  private isOwner = false;
  private selectedLayer: Layer;
  private selectedToolboxOption = ToolboxOption.Selector;

  @ViewChild('canvasContainer') canvasContainer: ElementRef;
  application: Application;

  constructor(private gameState: GameStateService,
    private layerSerivce: LayerService,
    private mapService: MapService,
    private dragService: DragService) { super(); }

  ngOnInit() {
    this.initializeCanvas();

    this.gameState.isOwnerObservable.pipe(takeUntil(this.destroy))
      .subscribe(isOwner => {
        this.isOwner = isOwner;
        if (this.isOwner) {
          this.gameState.selectMapObservable.pipe(takeUntil(this.destroy))
            .subscribe(map => this.changeMap(map));
        } else {
          this.gameState.mapVisibilityChangedObservable.pipe(takeUntil(this.destroy))
            .subscribe(map => this.changeMap(map));
        }
      });

    this.gameState.selectLayerObservable
      .pipe(takeUntil(this.destroy))
      .subscribe(layer => {
        this.selectedLayer = layer;
      });
  }

  dragOver(event: DragEvent) {
    event.preventDefault();
  }

  drop(event: DragEvent) {
    event.preventDefault();
    const item = this.dragService.itemBeingDragged;
    // const token = TokenFactory.create(event.layerX, event.layerY, item);
    // const sprite = token.createToken(this.selectedLayer.order);
    // this.application.stage.addChild(sprite);

    // this.layerSerivce.addToken(token, this.selectedLayer.id)
    //   .pipe(takeUntil(this.destroy))
    //   .subscribe(res => console.log(res));
  }

  private initializeCanvas() {
    const div = this.canvasContainer.nativeElement as HTMLDivElement;
    this.application = new Application();
    this.application.renderer.backgroundColor = 0xFFFFFF;
    this.application.stage.sortableChildren = true;
    div.appendChild(this.application.view);
  }

  private changeMap(map: PlayMap): void {
    this.application.stage.removeChildren();
    this.mapService.getLayers(map.id)
      .pipe(takeUntil(this.destroy))
      .subscribe(layers => {
        layers.forEach(layer => {
          this.renderTokens(layer);
        });

      });
    const canvasWidth = map.widthInPixels;
    const canvasHeight = map.heightInPixels;

    this.application.renderer.resize(canvasWidth, canvasHeight);
    GridDrawer.drawGrid(map.gridSizeInPixels, this.application);
  }

  private renderTokens(layer: Layer) {
    layer.tokens.forEach(token => {
      const sprite = SpriteFactory.create(<CharacterToken>token, layer.order);
      this.application.stage.addChild(sprite);
    });
  }
}
