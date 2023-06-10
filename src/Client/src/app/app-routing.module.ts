import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { ChatsComponent } from './components/chats/chats.component';

const routes: Routes = [
  { path: 'login', component: AuthComponent },
  { path: 'chats', component: ChatsComponent },
  { path: '**', redirectTo: 'chats' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
