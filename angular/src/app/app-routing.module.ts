import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameComponent } from './game/game.component';
import { AccountComponent } from './account/account.component';
import { AuthGuard } from './shared/authguard';
import { CreateGameComponent } from './game-catalog/create-game/create-game.component';

const routes: Routes = [
  { path: '', component: AccountComponent },
  { path: 'chooseGame', component: CreateGameComponent, canActivate: [AuthGuard] },
  { path: 'game/:id', component: GameComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }
