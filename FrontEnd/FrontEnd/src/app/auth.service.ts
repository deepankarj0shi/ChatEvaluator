import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:7247/api/auth';
  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password });
  }

  register(username: string, password: string) {
    return this.http.post<any>(`${this.apiUrl}/register`, { username, password });
  }

  saveToken(token: string) { localStorage.setItem('jwt', token); }
  getToken() { return localStorage.getItem('jwt'); }
  getUsername() { return localStorage.getItem('username'); }
  saveUsername(username: string) { localStorage.setItem('username', username); }
  logout() {
    localStorage.removeItem('jwt');
    localStorage.removeItem('username');
    this.router.navigate(['/login']);
  }
  isLoggedIn() { return !!this.getToken(); }
}
