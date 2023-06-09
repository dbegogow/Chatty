import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ChatModel } from '../models/response/chat.model';
import { environment } from '../../environments/environment';
import { MessageModel } from '../models/request/message.model';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private http: HttpClient) { }

  chats(): Observable<ChatModel[]> {
    return this.http.get<ChatModel[]>(
      `${environment.apiUrl}/api/chats`);
  }

  search(username: string, skip: number, take: number): Observable<ChatModel[]> {
    return this.http.get<ChatModel[]>(
      `${environment.apiUrl}/api/chats/search?username=${username}&skip=${skip}&take=${take}`);
  }

  sendMessage(model: MessageModel): Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/api/chat/send-message`, model);
  }
}
