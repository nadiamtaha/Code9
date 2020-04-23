import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from  '@angular/forms';
import { Router } from  '@angular/router';
import { NgxSpinnerService } from "ngx-spinner";
import { ToastrService } from 'ngx-toastr';
import { NgwWowService } from 'ngx-wow';
import { AuthenticationService } from 'src/app/services/authenticaation.service';
declare var $ :any;
import * as moment from 'moment';

import Swal from 'sweetalert2/dist/sweetalert2.js'

@Component({
  selector: 'app-medical-status',
  templateUrl: './medical-status.component.html',
  styleUrls: ['./medical-status.component.css']
})

export class MedicalStatusComponent implements OnInit {
  isSearchClick:boolean=false;
  isCitizenSelected:boolean=true;
  normal:boolean=false;
  suspecious:boolean=false;
  infected:boolean=false;
  data:any={};
  searchTxt;
  searchModel:any={};
  statusModel:any={};

  constructor(private router: Router, private formBuilder: FormBuilder,
    private _AuthenticationService:AuthenticationService,private wowService: NgwWowService,
    private spinner: NgxSpinnerService,private toastr: ToastrService ) { 
      this.wowService.init();

  }
  onStatusChange(status){
    if(status=="infected"){
        this.infected=true;
        this.normal=false;
        this.suspecious=false;
        this.statusModel.userStatusEnum=3
    }
    else if(status=="suspecious")
    {
      this.infected=false;
      this.normal=false;
      this.suspecious=true;
      this.statusModel.userStatusEnum=2

    }
    else{
      this.infected=false;
      this.normal=true;
      this.suspecious=false;
      this.statusModel.userStatusEnum=1

    }
  }
  activeBtn(e)
  {
    this.searchTxt="";
    this.isSearchClick=false
    this.infected=false;
    this.normal=false;
    this.suspecious=false;
    if(e.target.innerText=="Citizen")
    {
      $("#citizin").addClass("active-btn");
      $("#shop").removeClass("active-btn")
      this.isCitizenSelected=true;
    }
    else{
      $("#shop").addClass("active-btn")
      $("#citizin").removeClass("active-btn");
      this.isCitizenSelected=false

    }
  }

  search(){
    this.spinner.show();
    if(this.isCitizenSelected){
      this.searchModel.userType=1;
    }
    else{
      this.searchModel.userType=2;
    }
    this.isSearchClick=true;
    if(this.searchTxt==undefined)
    this.searchTxt="";
    this.searchModel.id=this.searchTxt;

    this._AuthenticationService.search(this.searchModel).subscribe({next:response=>{
      if(response.isSuccess){
        this.isSearchClick=true;
        this.data=response.data[0];
        if(this.data.dateOfBirth)

        {
          var date = new Date(this.data.dateOfBirth);
          this.data.dateOfBirth=moment(date.toISOString().slice(0,10)).format('DD-MM-YYYY');
          //this.data.dateOfBirth.toISOString().slice(0,10);

        }
       }
       else{
        this.toastr.error(response.errors[0]);
        this.isSearchClick=false;

       }
       this.spinner.hide();

    },
    error:err=>{
      this.isSearchClick=false;
      this.spinner.hide();
      this.toastr.error('حدث خطأ ما !');
      console.log(err.error);

      
    }
    
    })
  }
  editStatus(){
    this.spinner.show();
   // if(isNumber(this.data.id))
   this.statusModel.userId = this.data.id.toString();
    // if (this.data.licenseNumber)
    //   this.statusModel.userId=this.data.idNumber
    this.statusModel.userType= this.searchModel.userType;
     this._AuthenticationService.editStatus(this.statusModel).subscribe({next:response=>{
      if(response.isSuccess){
        this.toastr.success("Status updated succesfully");
        $("#exampleModal").modal('hide');

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
  }
  logout(){
    localStorage.removeItem("currentUser")
    this.router.navigateByUrl(`/login`)
  }
  openStatus(){
    
    $("#exampleModal").modal('show');



  }

  onLogoutIconClicked(){
    Swal.fire({
      title: '',
      text:  `Are you sure you want to logout?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Submit',
      cancelButtonText: 'Cancel'
    }).then((result) => {
      if (result.value) {
        this.logout();

      }
    })
   
  }
  ngOnInit() {
  }

}
