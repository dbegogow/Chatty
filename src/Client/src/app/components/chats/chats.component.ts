import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ChatService } from '../../services/chat.service';
import { ChatsSearch } from '../../models/response/chats-serach.model';

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

  chats: ChatsSearch[] = [];
  openedChat!: ChatsSearch;
  isChatsLoading: boolean = false;

  constructor(
    private toastr: ToastrService,
    private chatService: ChatService) { }

  ngOnInit() {
    this.allChats();
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
    this.isChatsLoading = true;

    this.chatService.chats()
      .subscribe({
        next: res => {
          this.chats = res;

          if (this.chats.length > 0) {
            this.messagesScrollToBottom();
          }
        },
        error: () => {
          this.toastr.error('Error occured');
        },
      }).add(() => {
        this.isChatsLoading = false;
      });
  }

  openChat(username: string, profileImageUrl: string, closeSearchBar: boolean) {
    if (closeSearchBar) {
      this.isChatsBarClosed = true;
    }

    const chat = this.chats.find(c => c.username === username);

    if (chat) {
      this.openedChat = chat;
    } else {
      const newChat: ChatsSearch = {
        profileImageUrl: profileImageUrl,
        username: username
      };

      this.openedChat = newChat;
      this.chats.unshift(newChat);
    }
  }

  messagesScrollToBottom() {
    const scrollableElement = this.scrollableElementRef.nativeElement;
    scrollableElement.scrollTop = scrollableElement.scrollHeight;
  }
}
