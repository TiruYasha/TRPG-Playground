import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameCatalogComponent } from './game-catalog.component';
import { CreateGameComponent } from './create-game/create-game.component';
import { GameCatalogItemComponent } from './game-catalog-item/game-catalog-item.component';

@NgModule({
  declarations: [GameCatalogComponent, CreateGameComponent, GameCatalogItemComponent],
  imports: [
    CommonModule
  ]
})
export class GameCatalogModule { }
