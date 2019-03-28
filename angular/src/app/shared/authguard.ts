import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AccountService } from './services/account.service';
@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private accountService: AccountService, private router: Router) { }
    canActivate(): boolean {
        if (!this.accountService.isAuthenticated()) {
            this.router.navigate(['']);
            return false;
        }
        return true;
    }
}
