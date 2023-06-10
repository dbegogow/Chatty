import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { RegisterModel } from '../models/request/register.model';
import { IdentityModel } from '../models/response/identity.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  register(model: RegisterModel): Observable<IdentityModel> {
    return this.http.post<IdentityModel>(`${environment.apiUrl}/api/register`, model);
  }
}
