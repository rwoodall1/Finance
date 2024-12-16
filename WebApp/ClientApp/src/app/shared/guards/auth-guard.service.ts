import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from "@auth0/angular-jwt";
@Injectable()

export class AuthGuard  {
  
  constructor(private router: Router) { }
  jwtService = new JwtHelperService();
  token = localStorage.getItem("jwt");
   
    canActivate() {
    
      const token = localStorage.getItem("jwt");
 
        if (token && !this.jwtService.isTokenExpired(token)) {
            return true;
      }
    
        this.router.navigate(["login"]);
        return true;
    }
}
