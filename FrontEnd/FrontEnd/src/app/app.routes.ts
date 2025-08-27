import { Routes } from '@angular/router';
import { LoginComponent } from '../app/login/login.component';
import { ChatComponent } from '../app/chat/chat.component';
import { RegisterComponent } from './register/register.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'chat', component: ChatComponent },
  { path: 'register', component: RegisterComponent }
];
