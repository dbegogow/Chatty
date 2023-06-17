import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ChatService } from '../../services/chat.service';
import { ChatsSearch } from '../../models/response/chatsSerach.model';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css']
})
export class ChatsComponent implements OnInit {
  @ViewChild('scrollableMessages', { static: true }) scrollableElementRef!: ElementRef;

  isChatsBarClosed: boolean = true;
  isChatsBarLoading: boolean = false;

  chatsBar: ChatsSearch[] = [];

  constructor(
    private toastr: ToastrService,
    private chatService: ChatService) { }

  ngOnInit() {
    this.messagesScrollToBottom();
  }

  searchChats(event: any) {
    const value = event.target.value;

    if (!value) {
      this.isChatsBarClosed = true;
      return;
    }

    this.isChatsBarClosed = false;
    this.isChatsBarLoading = true;

    this.chatService.search(value)
      .subscribe({
        next: res => {
          this.chatsBar = res;
          console.log(this.chatsBar);
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
