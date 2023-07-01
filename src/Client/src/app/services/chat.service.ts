import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatsSearch } from '../models/response/chats-serach.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient) { }

  search(username: string, skip: number, take: number): Observable<ChatsSearch[]> {
    return this.http.get<ChatsSearch[]>(
      `${environment.apiUrl}/api/chats/search?username=${username}&skip=${skip}&take=${take}`);
  }
}
