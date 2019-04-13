import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';

import { KonvaModule } from 'ng2-konva';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from './http-interceptors/token-interceptor';
import { AppRoutingModule } from './app-routing.module';
import { GameModule } from './game/game.module';
import { FormsModule } from '@angular/forms';
import { AccountComponent } from './account/account.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthGuard } from './shared/authguard';
import { GameCatalogModule } from './game-catalog/game-catalog.module';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  declarations: [
    AppComponent,
    AccountComponent,
  ],
  imports: [
    BrowserModule,
    KonvaModule,
    HttpClientModule,
    AppRoutingModule,
    GameModule,
    GameCatalogModule,
    FormsModule,
    BrowserAnimationsModule,
    MatIconModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
