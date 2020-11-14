import { SignInModel } from './../Models/sign-in.model';
import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SignUpModel } from '../Models/sign-up.model';
import { ApiResponse } from './../Models/api.responce';
import jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private router: Router) { }

  statusLogin = new EventEmitter<boolean>();

  baseUrl = "/api/Account";
  SingUp(model: SignUpModel): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.baseUrl + '/register', model);

  }
  SingIn(model: SignInModel): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.baseUrl + '/login', model);
  }

  isLoggedIn() {
    var token = localStorage.getItem('token');
    if (token != null) {
      return true;
    }
    else {
      return false;
    }
  }

  isAdmin() {
    if (this.isLoggedIn()) {
      var token = localStorage.getItem('token');
      var dataToken = jwt_decode(token);
      if (dataToken.roles === "Admin") {
        return true;
      }
      else {
        return false;
      }
    }
  }
  LogOut() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
    this.statusLogin.emit(false);
  }
}
