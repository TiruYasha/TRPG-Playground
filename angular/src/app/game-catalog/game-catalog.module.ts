import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameCatalogComponent } from './game-catalog.component';
import { CreateGameComponent } from './create-game/create-game.component';
import { GameCatalogItemComponent } from './game-catalog-item/game-catalog-item.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [GameCatalogComponent, CreateGameComponent, GameCatalogItemComponent],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    GameCatalogComponent
  ]
})
export class GameCatalogModule { }
