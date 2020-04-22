import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  baseUrl:any;
  loginUrl:any;
  registerUrl:any;
  codeUrl:any
  constructor(private http:HttpClient,public appConfigService: AppConfigService) { 
    this.baseUrl=appConfigService.apiBaseUrl;
    this.loginUrl=this.baseUrl+'Account/Login';
    this.registerUrl=this.baseUrl+'Account/Register';
    this.codeUrl=this.baseUrl+'Account/CheckVerificationCode';
  }

  

  public login(user):Observable<any>
  {  
    //var credintials =`username=${user.username}&password=${user.password}`;
    return this.http.post(this.loginUrl,user)
  }
 
  public register(user):Observable<any>
  {  
    //var credintials =`username=${user.username}&password=${user.password}`;
    return this.http.post(this.registerUrl,user)
  }
  public checkVerificationCode(user):Observable<any>
  {  
    //var credintials =`username=${user.username}&password=${user.password}`;
    return this.http.post(this.codeUrl,user)
  }
  //test api
  // public getVerificationCodeByUserName(user):Observable<any>
  // {  
  //   //var credintials =`username=${user.username}&password=${user.password}`;
  //   return this.http.post(this.registerUrl,user)
  // }
}
