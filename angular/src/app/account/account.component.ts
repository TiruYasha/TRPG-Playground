import { Component, OnInit } from '@angular/core';
import { AccountService } from '../shared/services/account.service';
import { Router } from '@angular/router';
import { RegisterModel } from '../shared/models/account/register.model';
import { LoginModel } from '../shared/models/account/login.model';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  registerModel: RegisterModel = new RegisterModel();
  loginModel: LoginModel = new LoginModel();

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('token') !== null) {
      this.router.navigate(['chooseGame']);
    }
  }

  onRegister() {
    this.accountService.register(this.registerModel)
      .subscribe(data => {
        console.log('registration successfull');
      });
  }

  onLogin() {
    this.accountService.login(this.loginModel).subscribe(data => this.finalizeLogin(data));
  }

  finalizeLogin(data) {
    console.log('login successfull', data);
    localStorage.setItem('token', data);
    this.router.navigate(['chooseGame']);
  }
}
