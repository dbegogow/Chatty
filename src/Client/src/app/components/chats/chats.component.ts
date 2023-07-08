import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ChatService } from '../../services/chat.service';
import { ChatModel } from '../../models/response/chat.model';
import { MessageModel } from 'src/app/models/request/message.model';
import { AuthService } from 'src/app/services/auth.service';

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
  chatsBar: ChatModel[] = [];

  chats: ChatModel[] = [];
  openedChat: ChatModel | null = null;
  isChatsLoading: boolean = false;

  messageText: string = '';

  profileImageUrl!: string | null;

  constructor(
    private toastr: ToastrService,
    private chatService: ChatService,
    private authService: AuthService) { }

  ngOnInit() {
    this.profileImageUrl = this.authService.getProfileImageUrl();
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
        },
        error: () => {
          this.toastr.error('Error occured');
        },
      }).add(() => {
        this.isChatsLoading = false;
      });
  }

  sendMessage() {
    if (!this.messageText || !this.openedChat) {
      return;
    }

    const message: MessageModel = {
      receiverUsername: this.openedChat?.username,
      text: this.messageText
    };

    this.chatService.sendMessage(message)
      .subscribe({
        next: () => {
          this.messageText = '';
        },
        error: () => {
          this.toastr.error('Error occured');
        },
      });
  }

  openChat(username: string, closeSearchBar: boolean = false, profileImageUrl: string = '') {
    if (closeSearchBar) {
      this.isChatsBarClosed = true;
    }

    const chat = this.chats.find(c => c.username === username);

    if (chat) {
      this.openedChat = chat;
    } else {
      const newChat: ChatModel = {
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
