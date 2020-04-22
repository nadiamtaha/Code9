import { Injectable } from '@angular/core';
import Swal from 'sweetalert2/dist/sweetalert2.js'
import { FormControl } from  '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class HelperService {

  showDeleteConfirmationMessage(params,successFun){
    Swal.fire({
      title: 'هل انت متاكد؟',
      text: `سوف يتم حذف  ${params}`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'تاكيد !',
      cancelButtonText: 'الغاء'
    }).then((result) => {
      if (result.value) {
        successFun();
        Swal.fire(
          'تم الحذف بنجاح !',
        )
       
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        // Swal.fire(
        //   'تم الغاء الحذف !',
        // )
      }
    })
  }
  constructor() { }

  noWhitespaceValidator(control: FormControl) {
    const isWhitespace = (control && control.value && control.value.toString() || '').trim().length === 0;
    const isValid = !isWhitespace;
    return isValid ? null : { 'whitespace': true };
  }

  hideMenu(){
    $("#sidebar-overlay").removeClass("app-overlay");
    $("#wrapper").toggleClass("toggled");
    $(".toggle-icon").toggleClass("icon-toggled")
  }
}
