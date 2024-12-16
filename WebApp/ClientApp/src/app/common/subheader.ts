import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { EmitterService } from '../services/emitter.service';
import {  Router} from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser'
import { LoggedInUser } from '../bindingmodels/userBindingModel';
import { GlobalService } from '../services/Services';

import { NotificationService } from '../services/notification.service'

import * as moment from 'moment';
import { environment } from '../../environments/environment';
import { from } from 'rxjs';
import { Observable,of } from 'rxjs';


@Component({
    selector: 'app-subheader',
    encapsulation: ViewEncapsulation.None,
    templateUrl: './subheader.html',
    styleUrls: ['./subheader.css'],
    standalone: false
})
export class SubHeaderComponent implements OnInit {
  
  user:Observable<LoggedInUser>;
  environment = environment;
  isAdmin: boolean;
  isAdviser: boolean;
  private subscription: any;
  impersonatedUser: LoggedInUser;
  environmentArea: string;
  constructor(public location: Location, private router: Router, public _global: GlobalService, private Notification: NotificationService, private domSanitizer: DomSanitizer) {
let a=environment.production
    //this.subscription = this._global.getImpersonatedChangedEmmitter().subscribe(() => {
    //  this.impersonatedUser = this._global.getImpersonatedUser();

   /* })*/
   
  }
  
  ngOnInit(): void {
    this.environmentArea = environment.uppEnvironment.toUpperCase();
    //this.impersonatedUser = this._global.getImpersonatedUser();
    //this.user = of(this._global.getLoggedInUser());
    //this.user.subscribe(val => {
    //  this.schoolName = val.schName
    //  this.schcode = val.schcode;
    //  this.isAdmin = val.isAdmin;
    //  this.isAdviser = val.isAdviser;
    //})
    
  }
  //clearImpersonation() {
  //  this._global.clearImpersonatedUser();
  //}
  


}
