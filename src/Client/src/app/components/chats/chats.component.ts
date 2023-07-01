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

  chats: 

  constructor(
    private toastr: ToastrService,
    private chatService: ChatService) { }

  ngOnInit() {
    this.messagesScrollToBottom();
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

  messagesScrollToBottom() {
    const scrollableElement = this.scrollableElementRef.nativeElement;
    scrollableElement.scrollTop = scrollableElement.scrollHeight;
  }
}
