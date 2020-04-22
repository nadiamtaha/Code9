import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2/dist/sweetalert2.js'

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
    constructor(private router:Router,private toastr: ToastrService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with basic auth credentials if available
        const currentUser = JSON.parse(localStorage.getItem("currentUser"));
        if (currentUser && currentUser.data.token) {
            request = request.clone({
                setHeaders: { 
                    Authorization: `Bearer ${currentUser.data.token}`
                }
            });
        }
       //handle errors
        return next.handle(request).pipe(
            catchError(error => {
            if (error.status === 401) {
                
                localStorage.removeItem("currentUser")
                this.router.navigateByUrl(`/login`)
            //   return next.handle(request);
            }
            else if(error.status === 403){
                Swal.fire(
                    'لا يوجد صلاحيات لهذا المستخدم',
                  )
                  this.router.navigate(['/dashboard']);

            }
            else {
              this.toastr.error("حدث خطأ في التواصل مع الخادم");
              return throwError(error);
            }
          })
          );

        
    }
}