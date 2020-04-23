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
  searchUrl:any;
  editStatusUrl:any;
  getStatusUrl:any;

  constructor(private http:HttpClient,public appConfigService: AppConfigService) { 
    this.baseUrl=appConfigService.apiBaseUrl;
    this.loginUrl=this.baseUrl+'Account/Login';
    this.searchUrl=this.baseUrl+'Admin/Search';
    this.editStatusUrl=this.baseUrl+'Admin/EditStatus';
    this.getStatusUrl=this.baseUrl+'Dashbord/GetDashboardData';
  }

  public search(model):Observable<any>
  {  
    //var credintials =`username=${user.username}&password=${user.password}`;
    return this.http.post(this.searchUrl,model)
  }

  public login(user):Observable<any>
  {  
    //var credintials =`username=${user.username}&password=${user.password}`;
    return this.http.post(this.loginUrl,user)
  }
  public editStatus(status):Observable<any>
  {  
    //var credintials =`username=${user.username}&password=${user.password}`;
    return this.http.post(this.editStatusUrl,status)
  }
 public getStatus(user):Observable<any>
 {
  return this.http.post(this.getStatusUrl,user)

 }
  //test api
  // public getVerificationCodeByUserName(user):Observable<any>
  // {  
  //   //var credintials =`username=${user.username}&password=${user.password}`;
  //   return this.http.post(this.registerUrl,user)
  // }
}
