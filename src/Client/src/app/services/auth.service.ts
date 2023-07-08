import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { RegisterModel } from '../models/request/register.model';
import { LoginModel } from '../models/request/login.model';
import { IdentityModel } from '../models/response/identity.model';
import { IdentityLoginModel } from '../models/response/identity-login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private jtwHelper: JwtHelperService,) { }

  register(model: RegisterModel): Observable<IdentityModel> {
    return this.http.post<IdentityModel>(`${environment.apiUrl}/api/register`, model);
  }

  login(model: LoginModel): Observable<IdentityLoginModel> {
    return this.http.post<IdentityLoginModel>(`${environment.apiUrl}/api/login`, model);
  }

  saveToken(token: string) {
    localStorage.setItem('access_token', token);
  }

  getToken() {
    return localStorage.getItem('access_token');
  }

  isAuthenticated() {
    const token = this.getToken();

    const isTokenValid = token
      ? !this.jtwHelper.isTokenExpired(token)
      : false;

    return isTokenValid;
  }

  saveProfileImageUrl(profileImageUrl: string) {
    localStorage.setItem('profile_image_url', profileImageUrl);
  }

  getProfileImageUrl() {
    return localStorage.getItem('profile_image_url');
  }
}
