import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from "@angular/router";
import { NgForm, UntypedFormControl } from '@angular/forms';
import { BrowserModule, DomSanitizer } from '@angular/platform-browser'
import { GlobalService,AuthService } from '../../services/Services';
import { LoggedInUser, LogInData } from '../../bindingmodels/userBindingModel'
import { Observable } from 'rxjs';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { NotificationService } from '../../services/notification.service';

import { environment } from '../../../environments/environment';
import { ModalService } from '../../shared/modal/modal.service';
import {  MatDialog,MatDialogConfig, MatDialogModule } from '@angular/material/dialog';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    standalone: false
})
export class LoginComponent {

  invalidLogin: boolean;
  Token: string;
  styles: string;
  loginData = new LogInData();
  showSpinner = false;
  showForm = "login"
  constructor(public dialog: MatDialog,private authService:AuthService,  private modalService: ModalService, private Global: GlobalService, private Notification: NotificationService, private router: Router, private http: HttpClient, private domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.styles = `<style>html, body { background-color:#e6eaed !important;}</style>`
  
  }
  login(form: NgForm) {
    this.showSpinner = true;
    this.SetControlsDirty(form);
    if (form.valid) {
      let credentials = JSON.stringify(this.loginData);
      const result = this.authService.authenticateUser(credentials).subscribe(response => {
        if (response.isError) {
          this.Notification.displayError(response.errors[0].errorMessage);
          this.invalidLogin = true;
          this.showSpinner = false;
          return;
        }
        this.invalidLogin = false;
        this.showSpinner = false;
        this.router.navigate(["/"]);
      });
    }
  }
  SetControlsDirty(form: NgForm) {
    for (var eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();
    }
  }
  setForm(formName: string) {
    this.showForm = formName;
  }
  getStyles() {
    return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
  }

  openModal(id: string) {
    this.modalService.open(id);
  }
  closeModal(id: string) {

    this.modalService.close(id);
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






