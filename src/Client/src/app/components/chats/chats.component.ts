import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styleUrls: ['./chats.component.css']
})
export class ChatsComponent implements OnInit {
  @ViewChild('scrollableMessages', { static: true }) scrollableElementRef!: ElementRef;


  ngOnInit() {
    this.messagesScrollToBottom();
  }

  messagesScrollToBottom() {
    const scrollableElement = this.scrollableElementRef.nativeElement;
    scrollableElement.scrollTop = scrollableElement.scrollHeight;
  }
}
