import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatWindowComponent } from './chat-window/chat-window.component';
import { FormsModule } from '@angular/forms';
import { MatInputModule, MatIconModule } from '@angular/material';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatListModule } from '@angular/material/list';
import { PlainTextComponent } from './message-components/plain-text/plain-text.component';
import { NormalRollResultComponent } from './message-components/normal-roll-result/normal-roll-result.component';
import { MatCardModule } from '@angular/material/card';
import { ErrorMessageComponent } from './message-components/error-text/error-message/error-message.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatListModule,
    MatCardModule,
    MatIconModule,
    BrowserAnimationsModule
  ],
  exports: [
    ChatWindowComponent
  ],
  declarations: [ChatWindowComponent, PlainTextComponent, NormalRollResultComponent, ErrorMessageComponent]
})
export class ChatModule { }
