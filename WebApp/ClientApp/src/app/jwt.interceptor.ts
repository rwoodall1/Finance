//https://medium.com/@ryanchenkie_40935/angular-authentication-using-the-http-client-and-http-interceptors-2f9d1540eb8
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { tap, map } from 'rxjs/operators';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
//import { AuthService } from './auth/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private router: Router) {

   
  }
 //This is left here for an example and is not being used.
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
   
    return next.handle(request).pipe(tap((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        // do stuff with response if you want
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          // redirect to the login route
          // or show a modal
          this.router.navigate(['login'])
        }
      }
    }));
  }
}
