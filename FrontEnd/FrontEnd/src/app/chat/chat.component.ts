import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChatService,ActivityLog } from '../chat.service';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule, HeaderComponent],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  expression = '';
  messages: { user: string, text: string }[] = [];

  constructor(private chatService: ChatService) {}

  ngOnInit() { this.loadHistory(); }

loadHistory() {
  this.chatService.getHistory().subscribe({
    next: (data: any) => {
      console.log("Raw history response:", data);

      // Normalize: if it's a single object, wrap in an array
      const historyArray = Array.isArray(data) ? data : [data];

      this.messages = historyArray.map((h: any) => ({
        user: 'You',
        text: `${h.expression} = ${h.result} (${h.resultCode})`
      }));

      console.log("Mapped messages:", this.messages);
    },
    error: (err) => console.error("Failed to load chat history", err)
  });
}
//  loadHistory() {
//   this.chatService.getHistory().subscribe({
//     next: (historyArray: ActivityLog[]) => {
//       this.messages = historyArray.map(h => ({
//         user: 'You',
//         text: `${h.expression} = ${h.result} (${h.resultCode})`
//       }));
//     },
//     error: (err) => {
//       console.error('Failed to load chat history', err);
//       this.messages = [];
//     }
//   });
// }
  sendMessage() {
    if (!this.expression.trim()) return;
    this.messages.push({ user: 'You', text: this.expression });
    this.chatService.calculate(this.expression).subscribe(res => {
      this.messages.push({ user: 'Bot', text: `${res.result} (${res.resultCode})` });
    });
    this.expression = '';
  }
}
