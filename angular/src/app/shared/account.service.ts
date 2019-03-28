import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterModel } from '../models/login.model';
import { environment } from 'src/environments/environment.prod';
import { Observable } from 'rxjs';
import { LoginSuccessModel } from '../models/loginsuccess.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private http: HttpClient) { }

  register(model: RegisterModel) {
    return this.http.post(environment.apiUrl + '/register', model);
  }

  login(model: RegisterModel): Observable<LoginSuccessModel> {
    return this.http.post<LoginSuccessModel>(environment.apiUrl + '/login', model);
  }
}
