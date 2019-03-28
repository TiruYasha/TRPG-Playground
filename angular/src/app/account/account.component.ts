import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../models/login.model';
import { LoginSuccessModel } from '../models/loginsuccess.model';
import { AccountService } from '../shared/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  registerModel: RegisterModel = new RegisterModel();
  loginModel: RegisterModel = new RegisterModel();

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
    localStorage.setItem('token', data.token);
    this.router.navigate(['chooseGame']);
  }
}
