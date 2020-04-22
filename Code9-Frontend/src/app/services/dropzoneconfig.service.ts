import { Injectable } from '@angular/core';
import { DropzoneModule, DropzoneConfigInterface } from 'ngx-dropzone-wrapper';
import { DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { environment } from 'src/environments/environment';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})


export class DropzoneconfigService {
  static getConfig(): any {
    const currentUser = JSON.parse(localStorage.getItem("currentUser"));
    if(currentUser){
      let DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
        // Change this to your upload POST address:
        
         url: environment.baseUrl+'SafetyNote/CreateSafetyNote',
         maxFilesize: 50,
         acceptedFiles: 'image/*',
         uploadMultiple:true,
         addRemoveLinks:true,
         
         headers: { 
          Authorization: `Bearer ${currentUser.data.token}`
         }
      };
      return DEFAULT_DROPZONE_CONFIG;
    }
    //throw new Error("Method not implemented.");
  }
  baseUrl:any;
  
  constructor(private DropzoneModule:DropzoneModule,public appConfigService: AppConfigService ) {
    this.baseUrl=appConfigService.apiBaseUrl
   }
  getConfig(){
   
   
  }
  

}
