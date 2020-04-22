import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery'
import { Router, ActivatedRoute } from '@angular/router';
import { HelperService } from '../services/helper.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  activeLink = false;
  currentUserRoles: any[];
  showBuilding: boolean;
  showNotes: boolean;
  showEvaluations: boolean;
  showReporting: boolean;
  showClassifications: boolean;
  showNotesTypes: boolean;
  showNotesClosure: boolean;
  showUsers: boolean;
  showDistribution:boolean;

  constructor(private router: Router,private _HelperService:HelperService) {

    if (JSON.parse(localStorage.getItem('currentUser')))
      this.currentUserRoles = JSON.parse(localStorage.getItem('currentUser')).data.roleViewModel;
    for (var i = 0; i < this.currentUserRoles.length; i++) {
      switch (this.currentUserRoles[i].roleEnglishName) {
        case "Building":
          this.showBuilding = true;
          break;
        case "SafetyNotes":
          this.showNotes = true;
          break;
        case "SafetyToolsEvaluation":
          this.showEvaluations = true;
          break;
        case "Reports":
          this.showReporting = true;
          break;
        case "SafetyToolsClassifications":
          this.showClassifications= true;
          break;
        case "SafetyNotesTypes":
          this.showNotesTypes = true;
          break;
        case "SafetyNetworkComponentDistributions":
          this.showDistribution = true;
          break;
        case "SafetyNoteClosure":
          this.showNotesClosure = true;
          break;
        case "UserManagement":
          this.showUsers = true;
          break;


      }
    }
  }

  public logout() {
    localStorage.removeItem("currentUser")
    this.router.navigateByUrl(`/login`)
    //environment.token=null;
  }
  ngOnInit() {
  }
  hideMenu(){
   this._HelperService.hideMenu()
  }
  toggleNavbar() {
    var width = $('.sidebar').width();
    if (width == 50) {
      $('.sidebar').animate({ width: "250" }, 1000);
      $('.content').animate({ marginRight: "250" }, 1000);
      $('.toggle-nav-icon').animate({ marginRight: "20" }, 1000);
      $('h3').css({ display: "block" });

      //  $('.content-overlay').css({opacity:"1",transition:'all 1s'});
      //  $('.content-overlay').css({zIndex:"5",transition:'all 1s'});



    } else {
      $('.sidebar').animate({ width: "50" }, 1000);
      $('.content').animate({ marginRight: "50" }, 1000);
      $('.toggle-nav-icon').animate({ marginRight: "20" }, 1000);
      $('h3').css({ display: "none" });

      //  $('.content-overlay').animate({opacity:"0"});
      //  $('.content-overlay').animate({zIndex:"-3"});


    }
  }

}
