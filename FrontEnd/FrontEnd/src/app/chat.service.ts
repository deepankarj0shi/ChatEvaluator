import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable,map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private apiUrl = 'https://localhost:7247/api/activitylog';
   
  constructor(private http: HttpClient) {}

  calculate(expression: string) {
    return this.http.post<any>(`${this.apiUrl}/submit`, JSON.stringify(expression), { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) });
  }

  getHistory() {
  return this.http.get<ActivityLog[]>(`${this.apiUrl}/history`);
  }
}
export interface ActivityLog {
  id: number;
  userId: number;
  expression: string;
  result: number;
  resultCode: string;
}
