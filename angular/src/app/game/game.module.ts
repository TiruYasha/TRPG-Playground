import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameComponent } from './game.component';
import { ChatModule } from './chat/chat.module';
import { FormsModule } from '@angular/forms';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTabsModule } from '@angular/material/tabs';
import { JournalModule } from './journal/journal.module';



@NgModule({
  imports: [
    CommonModule,
    ChatModule,
    FormsModule,
    MatSidenavModule,
    MatTabsModule,
    JournalModule
  ],
  declarations: [GameComponent]
})
export class GameModule { }
