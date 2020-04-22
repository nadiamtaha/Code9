import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BasicAuthInterceptor } from './interceptor';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { ApplayoutComponent } from './applayout/applayout.component';
import { LoginComponent } from './login/login.component';
import { SearchPipe } from './search.pipe';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxSpinnerModule } from "ngx-spinner";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from 'ngx-toastr';

import { DropzoneModule } from 'ngx-dropzone-wrapper';
import { DROPZONE_CONFIG } from 'ngx-dropzone-wrapper';
import { DropzoneConfigInterface } from 'ngx-dropzone-wrapper';
import {DataTablesModule} from 'angular-datatables';
import { AuthGuard } from './services/guards/auth-guard.service';
import { environment } from 'src/environments/environment';
import { DropzoneconfigService } from './services/dropzoneconfig.service';
import { AppConfigService } from './services/app-config.service';
import { DatePipe } from '@angular/common';




@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    ApplayoutComponent,
    LoginComponent,
   
  ],
  imports: [
    BrowserModule,
    //NgSelect2Module,
    AppRoutingModule,
    FormsModule, 
    ReactiveFormsModule,
    ChartsModule,
    HttpClientModule,
    NgSelectModule,
    NgxSpinnerModule,
    DataTablesModule,
    DropzoneModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-left',
      preventDuplicates: true,
      easing:'ease-in',
      progressAnimation:'decreasing'
    }) // ToastrModule added
  ],
  providers: [
    {
      provide: DROPZONE_CONFIG,
      useValue:DropzoneconfigService.getConfig()
    },
    AuthGuard,
    DatePipe,
     { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },

     {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [AppConfigService],
      useFactory: (appConfigService: AppConfigService) => {
        return () => {
          //Make sure to return a promise!
          return appConfigService.loadAppConfig();
        };
      }
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(public _DropzoneconfigService:DropzoneconfigService ){
     this._DropzoneconfigService.getConfig()
  }
}
