import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GameComponent } from './game/game.component';
import { AccountComponent } from './account/account.component';
import { AuthGuard } from './shared/authguard';
import { GameCatalogComponent } from './game-catalog/game-catalog.component';

const routes: Routes = [
  { path: '', component: AccountComponent },
  { path: 'chooseGame', component: GameCatalogComponent, canActivate: [AuthGuard] },
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
