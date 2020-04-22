import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery'
import { Router, ActivatedRoute } from  '@angular/router';
import { HelperService } from '../services/helper.service';

@Component({
  selector: 'app-applayout',
  templateUrl: './applayout.component.html',
  styleUrls: ['./applayout.component.css']
})
export class ApplayoutComponent implements OnInit {
  currentUserName:any;
  constructor(private router:Router,private _HelperService:HelperService) { 
    if(JSON.parse(localStorage.getItem('currentUser')))
  this.currentUserName=JSON.parse(localStorage.getItem('currentUser')).data.fullName;
  }

  ngOnInit() {
  }
  hideOverlay(){
    $('.sidebar').animate({width:"0"});
       $('.toggle-nav-icon').animate({marginLeft:"20"});
       $('.content-overlay').animate({opacity:"0"});
       $('.content-overlay').animate({zIndex:"-3"});
       $('.confirm-msg').css({opacity:"0"});

  }
  removeOverlay(){
    this._HelperService.hideMenu()


  }
  toggleNavbar() {
      $("#wrapper").toggleClass("toggled");
      $("#sidebar-overlay").toggleClass("app-overlay");
      $(".toggle-icon").toggleClass("icon-toggled")
    // var width = $('.sidebar').width();
    // if(width == 50) {
    //   $('.sidebar').animate({width:"250"},1000);
    //   $('.content').animate({marginRight:"250"},1000);
    //   $('.toggle-nav-icon').animate({marginRight:"20"},1000);
    //   $('h3').animate({opacity:"1",transition:'all 1s'});

      //  $('.content-overlay').css({opacity:"1",transition:'all 1s'});
      //  $('.content-overlay').css({zIndex:"5",transition:'all 1s'});

       
      
    // } else {
    //    $('.sidebar').animate({width:"50"},1000);
    //    $('.content').animate({marginRight:"50"},1000);
    //     $('.toggle-nav-icon').animate({marginRight:"20"},1000);
    //     $('h3').animate({opacity:"0",transition:'all 1s'});

    //   //  $('.content-overlay').animate({opacity:"0"});
    //   //  $('.content-overlay').animate({zIndex:"-3"});


    // }
  }
  logout(){
    localStorage.removeItem("currentUser")
    this.router.navigateByUrl(`/login`)
  }
}
