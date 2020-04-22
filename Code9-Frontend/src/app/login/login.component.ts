import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from  '@angular/router';
import { AuthenticationService } from '../services/authenticaation.service';
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from 'ngx-toastr';
import { NgwWowService } from 'ngx-wow';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  constructor(private router: Router, private formBuilder: FormBuilder,
    private _AuthenticationService:AuthenticationService,private wowService: NgwWowService,
    private spinner: NgxSpinnerService,private toastr: ToastrService ) { 
      this.wowService.init();

  }

  get formControls() { return this.loginForm.controls; }

  login(){
    this.spinner.show();
    if(this.loginForm.invalid){
      return;
    }
    this.loginForm.value.UserType=1;
    this._AuthenticationService.login(this.loginForm.value).subscribe({next:response=>{

      if(response.isSuccess){
        localStorage.setItem('currentUser',JSON.stringify(response));
        this.toastr.success("login successfully");

        //this.router.navigateByUrl('/dashboard');
       }
       else{
        this.toastr.error(response.errors[0]);
        
       }
       this.spinner.hide();

    },
    error:err=>{
      this.spinner.hide();
      this.toastr.error('حدث خطأ ما !');
      console.log(err.error);

      
    }
    
    })

    //this._AuthenticationService.login(this.loginForm.value);
  }
 
  ngOnInit() {
    this.loginForm  =  this.formBuilder.group({
        id: ['', [Validators.required]],
        password: ['', Validators.required],

    });   
  

  }

}
