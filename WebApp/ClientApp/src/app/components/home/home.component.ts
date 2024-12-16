import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService } from '../../services/Services';

import { NotificationService } from '../../services/notification.service'
import { LoggedInUser } from '../../bindingmodels/userBindingModel';

import * as moment from 'moment';

import { Observable, of } from 'rxjs';
import {  MatDialog, MatDialogConfig } from '@angular/material/dialog';




@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: [],
    standalone: false
})
export class HomeComponent {

  isAuth: boolean;
  user: LoggedInUser;
 
  loaded: boolean;
  shutdown: boolean;
  hideElement = true;
  moment = moment;
 

  setElement() {
    this.hideElement = false;
  }

  constructor(public dialog: MatDialog, private router: Router,public Global: GlobalService, private Notification: NotificationService, ) {
    this.user = this.Global.getLoggedInUser();
  
  }
  ngOnInit() {
    this.loaded = true;
     this.hideElement = true;
    
  }
}
