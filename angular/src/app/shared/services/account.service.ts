import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';
import { Observable } from 'rxjs';
import { LoginModel } from '../../models/account/login.model';
import { RegisterModel } from '../../models/account/register.model';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  register(model: RegisterModel) {
    return this.http.post(environment.apiUrl + '/account/register', model);
  }

  login(model: LoginModel): Observable<string> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
    const options = { headers, responseType: 'text' as 'json' };
    return this.http.post<string>(environment.apiUrl + '/account/login', model, options);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');

    if (!token) {
      return false;
    }

    return !this.jwtHelper.isTokenExpired(token);
  }
}
