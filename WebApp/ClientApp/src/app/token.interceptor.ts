//https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
  HttpInterceptor,
     HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { NotificationService } from './services/notification.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private router: Router, public Notification: NotificationService) {


  }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  
      let a = localStorage.getItem("jwt")
        request = request.clone({
            setHeaders: {
            Authorization: 'Bearer ' + localStorage.getItem("jwt")
              
            }
        });

      return next.handle(request).pipe(tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          // do stuff with response if you want
        }
      }, (err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status === 401) {
            this.Notification.displayWarning("Your session has expired, please login.")
            this.router.navigate(["login"]);
            return true;
            // redirect to the login route
            // or show a modal
         
          }
        }
      }));
    }
}
