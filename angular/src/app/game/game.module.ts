import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameComponent } from './game.component';
import { ChatModule } from './chat/chat.module';
import { FormsModule } from '@angular/forms';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule, MatCardModule } from '@angular/material';
import { JournalModule } from './journal/journal.module';
import { PlayAreaModule } from './play-area/play-area.module';
import { MapManagerModule } from './map-manager/map-manager.module';

@NgModule({
  imports: [
    CommonModule,
    ChatModule,
    FormsModule,
    MatSidenavModule,
    MatTabsModule,
    JournalModule,
    MatCardModule,
    PlayAreaModule,
    MapManagerModule
  ],
  declarations: [GameComponent]
})
export class GameModule { }
