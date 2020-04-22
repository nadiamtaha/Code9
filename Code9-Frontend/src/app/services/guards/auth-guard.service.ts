
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, Route } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class AuthGuard implements CanActivate {
  public href: string = "";
  

  constructor(private _router: Router) {

  }
  checkUrl(data,value){
    return data.some(function(obj) {
      return obj.roleEnglishName == value;
  });
 }
 
  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    const currentUser = JSON.parse(localStorage.getItem("currentUser"));
    //handle unauthorized use by roles

    // var routeArray=
    // [

    // "dashboard","building-report","safety-notes-report","login",
    // "safety-evaluation-report","add-Building","add-SafetyToolsClassification",
    // "add-SafetyNotesType","add-SafetyNetworkComponentDistribution","add-SafetyNote",
    // "add-SafetyToolsEvaluation","add-user"
    // ]
    // if(currentUser){
    //   var result;
    //   var currentUrl=window.location.href.split("/").pop();
      
    //   if (routeArray.some(v => currentUrl.includes(v))) {
    //     result=true;
    //   }
    //   // if(currentUrl=="dashboard"||currentUrl=="building-report"
    //   // ||currentUrl=="safety-notes-report"||currentUrl=="safety-evaluation-report")
    //   //  result=true;
    //   else 
    //    result=this.checkUrl(currentUser.data.roleViewModel,currentUrl)
    // }
  
    if (currentUser && currentUser.data.token) 
    {
        return true;
    }

    // navigate to login page
  
    this._router.navigate(['/login']);
    // you can save redirect url so after authing we can move them back to the page they requested
    return false;
  }

}
