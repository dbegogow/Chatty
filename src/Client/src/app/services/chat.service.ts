import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatsSearch } from '../models/response/chatsSerach.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient) { }

  search(username: string): Observable<ChatsSearch> {
    return this.http.get<ChatsSearch>(`${environment.apiUrl}/api/chats/search?username=${username}`);
  }
}
