import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ApplayoutComponent } from './applayout/applayout.component';
import { LoginComponent } from './login/login.component';

import { AuthGuard } from './services/guards/auth-guard.service';



export const routes: Routes = [
  
  
  { path: '', component: LoginComponent},
  { path: 'login', component: LoginComponent},

]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
