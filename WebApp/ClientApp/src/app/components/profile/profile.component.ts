import { Component, ViewChild, ViewEncapsulation, Input, Attribute, OnChanges, OnInit, Inject, ElementRef, NgModule, EventEmitter, Output, ChangeDetectorRef } from '@angular/core';
import { BrowserModule, DomSanitizer } from '@angular/platform-browser';
import { UserService } from '../../services/Services';
import { NotificationService } from '../../services/notification.service'
import { GlobalService } from '../../services/Services';
import { NewUserModel, UserProfile, LoggedInUser } from '../../bindingmodels/userBindingModel'
import { Location } from '@angular/common';
import { NgForm, UntypedFormControl } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { State } from '../../bindingmodels/miscBindingModels';
import { Constants } from '../../shared/global';
@Component({
    selector: "app-profile",
    templateUrl: "./profile.component.html",
    styleUrls: ['./profile.css'],
    standalone: false
})

export class ProfileComponent implements OnInit {
  styles: string;
  hasLoaded: boolean=false;
  user: UserProfile;
  userId: string;
  schcode: string;
  loggedInUser: LoggedInUser;
  billStates: State[]
  shipStates: State[]
  saving = false;
  newPassword: string='';
  passWordNotValid= false;
  constructor(public _location: Location, private constants: Constants, private activatedRoute: ActivatedRoute, private userService: UserService, private router: Router, public Global: GlobalService, private Notification: NotificationService) {
    this.loggedInUser = Global.getLoggedInUser();

  }
  ngOnInit(): void {
    this.billStates = this.constants.states;
    this.shipStates = this.constants.states;
    let tmpUserId = this.activatedRoute.snapshot.queryParamMap.get('id')

    if (tmpUserId) {
      this.userId = tmpUserId;

    } else {
      this.userId = this.loggedInUser.id;

    }
    this.getUser();



  }

  //getStyles() {
  //  return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
  //}
  getUser() {
    this.userService.getUserProfile(this.userId).subscribe(response => {
      const dataresponse: ApiProcessingResult<UserProfile> = response.apiProcessingResult;

      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.hasLoaded = true;
        return;
      }
      this.user = dataresponse.data;
      this.hasLoaded = true;
    })
  }
  save(form: NgForm) {
    this.SetControlsDirty(form);
    let lc = this;
    this.checkPasswordLength(form);
    if (form.valid) {
      
        this.user.password = this.newPassword;
      

      this.userService.updateUserProfile(this.user).subscribe(response => {

        let dataresponse = response.apiProcessingResult;

        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);

          return;
        }
        this.Notification.displaySuccess("Profile has been updated")
        this._location.back();

      });
    }
  }
  SetControlsDirty(form: NgForm) {
    for (let eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();
      (<UntypedFormControl>form.controls[eachControl]).markAllAsTouched();
    }
  }

  deleteUser() {
    this.saving = true;
    if (this.user.userName.toUpperCase()=='SA') {
      return;
    }
    this.userService.deleteUser(this.user.id).subscribe(response => {
      let dataresponse = response.apiProcessingResult;

      if (dataresponse.isError) {
        console.log(dataresponse.errors[0].errorMessage)
        this.Notification.displayError("Failed to remove user. See logs for reason.");
        this.saving = false;
        return;
      }
      this.Notification.displaySuccess("User has been removed")

      this.saving = false;


    })

  }
  checkPasswordLength(form: NgForm) {
    if (this.newPassword.length == 0 || this.newPassword.length > 4) {
      this.passWordNotValid= false
      form.controls['password'].setErrors(null);
    } else {
      this.passWordNotValid = true
      form.controls['password'].setErrors({length:true});
    }

  }
}

