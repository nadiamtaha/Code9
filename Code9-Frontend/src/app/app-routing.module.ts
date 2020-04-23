import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ApplayoutComponent } from './applayout/applayout.component';
import { LoginComponent } from './login/login.component';

import { AuthGuard } from './services/guards/auth-guard.service';
import { MedicalStatusComponent } from './modules/medical-status/medical-status.component';



export const routes: Routes = [
  
  {path:'MedicalStatus',component:MedicalStatusComponent,canActivate: [AuthGuard]},
  { path: '', component: MedicalStatusComponent,canActivate: [AuthGuard]},
  { path: 'login', component: LoginComponent},

]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
