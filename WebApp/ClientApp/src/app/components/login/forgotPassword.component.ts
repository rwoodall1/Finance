import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm, UntypedFormControl } from '@angular/forms';
import { BrowserModule, DomSanitizer } from '@angular/platform-browser'
import { GlobalService } from '../../services/Services';
import { LoggedInUser,ForgotPasswordData  } from '../../bindingmodels/userBindingModel'
import { Observable } from 'rxjs';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { NotificationService } from '../../services/notification.service';

import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-forgotpassword',
    templateUrl: './forgotPassword.component.html',
    standalone: false
})
export class ForgotPasswordComponent {


  styles: string;
  forgotPasswordData = new ForgotPasswordData();
  showSpinner = false;
  @Output() showFormEvent = new EventEmitter<string>();
  constructor( private Global: GlobalService, private Notification: NotificationService, private router: Router, private http: HttpClient, private domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.styles = `<style>html, body { background-color:#e6eaed !important;}</style>`
    
  }
  sendEmail(form: NgForm) {
    this.showSpinner = true;
    this.SetControlsDirty(form);
    if (form.valid) {
      let data = JSON.stringify(this.forgotPasswordData);
      this.http.post<ApiProcessingResult<boolean>>(environment.forgotPasswordUrl, data, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {

        let result = response.apiProcessingResult;
        if (result.isError) {
          this.Notification.displayError(result.errors[0].errorMessage)
          this.showSpinner = false;
          return;
        }
       
        this.showSpinner = false;
        this.showFormEvent.emit('login');
        this.Notification.displaySuccess("An email has been sent to you with your password.")

      }, err => {
        this.showSpinner = false;
        console.log(err.error)
        this.Notification.displayError(err.error);
      
      });
    } else {
      this.showSpinner = false;
    }
  }
  backToLogin() {
    this.showFormEvent.emit('login');

  }
  SetControlsDirty(form: NgForm) {
    for (var eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();
    }
  }
  getStyles() {
    return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
  }
}













////import { HttpClient, HttpHeaders } from '@angular/common/http';
////import { FormsModule,NgForm,FormControl } from '@angular/forms';

////import { Component } from '@angular/core';
////import { Router } from "@angular/router";

////import { BrowserModule, DomSanitizer } from '@angular/platform-browser'
//////import { GlobalService } from '../../services/Services';
////import { LoggedInUser, LogInData } from '../../bindingmodels/userBindingModel'
////import { Observable } from 'rxjs';
////import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
//////import { NotificationService } from '../../services/notification.service';

////@Component({
////  selector: 'login',
////  templateUrl: './login.component.html',
 
////})
////export class LoginComponent {

////  invalidLogin: boolean;
////  Token: string;
////  styles: string;
////  loginData = new LogInData();
////  constructor(/*private Global: GlobalService,*/ /*private Notification:NotificationService,*/private router: Router, private http: HttpClient, private domSanitizer: DomSanitizer) { }
////  ngOnInit(): void {
////    this.styles = `<style>html, body { background-color:#e6eaed !important;}</style>`
    
////  }
////  login(form:NgForm) {
  
////    this.SetControlsDirty(form);
////    if (form.valid) {
////      let credentials = JSON.stringify(this.loginData);

////      this.http.post<ApiProcessingResult<LoggedInUser>>("https://localhost:44406/auth/login", credentials, {
////        headers: new HttpHeaders({
////          "Content-Type": "application/json"
////        })
////      }).subscribe(response => {

////        let result = response.apiProcessingResult;
////        let token = result.data.token;
////        localStorage.setItem("jwt", token);
////        //this.Global.setLoggedInUser(result.data)

////        this.invalidLogin = false;
   
////        this.router.navigate(["/"]);
////      }, err => {
////        console.log(err.error)
////       // this.Notification.displayError(err.error);
////        this.invalidLogin = true;
////      });
////    }
////  }
////  SetControlsDirty(form:NgForm) {
////    for (var eachControl in form.controls) {
////      (<FormControl>form.controls[eachControl]).markAsDirty();
////    }
////  }
////  getStyles() {
////    return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
////  }
////}






