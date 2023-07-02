import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ChatService } from '../../services/chat.service';
import { ChatsSearch } from '../../models/response/chats-serach.model';
import { Chats } from 'src/app/models/response/chats.model';
import { Chat } from 'src/app/models/response/chat.model';
import { ChatUser } from 'src/app/models/response/chat-user.model';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css']
})
export class ChatsComponent implements OnInit {
  @ViewChild('scrollableMessages', { static: true }) scrollableElementRef!: ElementRef;

  isChatsBarClosed: boolean = true;
  isChatsBarLoading: boolean = false;

  searchUsername: string = '';
  searchSkip: number = 0;
  searchTake: number = 10;
  isSearchChatsNotCancelation: boolean = true;
  chatsBar: ChatsSearch[] = [];

  chats: Chats | null = null;
  openedChat!: Chat;

  constructor(
    private toastr: ToastrService,
    private chatService: ChatService) { }

  ngOnInit() {
    // this.allChats();
  }

  searchChats(resetBatch: boolean) {
    if (!this.searchUsername) {
      this.isChatsBarClosed = true;
      return;
    }

    if (resetBatch) {
      this.searchSkip = 0;
      this.chatsBar = [];
      this.isSearchChatsNotCancelation = true;
    } else {
      this.searchSkip += 10;
    }

    this.isChatsBarClosed = false;
    this.isChatsBarLoading = true;

    this.chatService.search(this.searchUsername, this.searchSkip, this.searchTake)
      .subscribe({
        next: res => {
          if (res.length === 0) {
            this.isSearchChatsNotCancelation = false;
            return;
          }

          this.chatsBar = this.chatsBar.concat(res);
        },
        error: () => {
          this.isChatsBarClosed = true;
          this.toastr.error('Error occured');
        },
      }).add(() => {
        this.isChatsBarLoading = false;
      });
  }

  allChats() {
    this.chatService.chats()
      .subscribe({
        next: res => {
          this.chats = res;

          if (this.chats.chats.length > 0) {
            this.messagesScrollToBottom();
          }
        },
        error: () => {
          this.toastr.error('Error occured');
        },
      });
  }

  openChat(username: string, profileImageUrl: string, closeSearchBar: boolean) {
    if (closeSearchBar) {
      this.isChatsBarClosed = true;
    }

    const chat = this.chats?.chats
      .find(c => c.users
        .some(u => u.username === username));

    if (chat) {
      this.openedChat = chat;
    } else {
      const chatUsers: ChatUser[] = [
        {
          profileImageUrl: profileImageUrl,
          username: username
        }
      ];

      const newChat = {
        users: chatUsers,
        messages: []
      };

      this.openedChat = newChat;
      this.chats?.chats.unshift(newChat);
    }
  }

  messagesScrollToBottom() {
    const scrollableElement = this.scrollableElementRef.nativeElement;
    scrollableElement.scrollTop = scrollableElement.scrollHeight;
  }
}
