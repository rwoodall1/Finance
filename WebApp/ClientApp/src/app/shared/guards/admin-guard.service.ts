import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from "@auth0/angular-jwt";
import { NotificationService } from '../../services/notification.service';
@Injectable()

export class AdminGuard  {
  constructor(private router: Router, public Notification: NotificationService) { }
  jwtService = new JwtHelperService();
  token = localStorage.getItem("jwt");
    canActivate() {
 
      const token = localStorage.getItem("jwt");
      if (!token || token && this.jwtService.isTokenExpired(token)) {
        this.Notification.displayWarning("Your session has expired. Please login again.")
        this.router.navigate(["login"]);
      } else {
        const data = this.jwtService.decodeToken(token)
        //1=Adviser, 0=Administrator
        if (data.Rank <= +2) {
          return true;
        } else {
         this.Notification.displayWarning("You do not have permission to go here, taking you to your home page")
          this.router.navigate(["home"]);
        }
      }
      return false;
    }
}
