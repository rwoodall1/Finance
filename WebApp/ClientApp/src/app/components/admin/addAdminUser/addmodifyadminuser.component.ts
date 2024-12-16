import { Component, ViewEncapsulation, ViewChild, Output, OnInit, Input, EventEmitter } from '@angular/core';
import { GlobalService } from '../../../services/Services';
import { NotificationService } from '../../../services/notification.service'
import { UserService } from '../../../services/Services'
import { LoggedInUser } from '../../../bindingmodels/userBindingModel';
import { ApiProcessingResult } from '../../../bindingmodels/coreBindingModel';
import { Router } from '@angular/router';
import { UserOfList, AdminUser } from '../../../bindingmodels/userBindingModel';
import { NgForm, UntypedFormControl } from '@angular/forms';
@Component({
    selector: 'app-addmodifyadminuser',
    templateUrl: './addmodifyadminuser.component.html',
    styleUrls: ['./addmodifyadminuser.component.css'],
    standalone: false
})
export class AddModifyAdminUserComponent implements OnInit {
  @Output() showSpinnerEvent = new EventEmitter<boolean>();
  @Output() refreshDataEvent = new EventEmitter();
  @Output() showListEvent = new EventEmitter<boolean>();
  @Input() addModify: string;
  @Input() currentUser: AdminUser;
  loggedInUser: LoggedInUser;
  newPassword: string = '';
  saving: boolean;
  hasLoaded: boolean = false;

  constructor(private userService: UserService, private router: Router, public Global: GlobalService, private Notification: NotificationService) {
    this.loggedInUser = this.Global.getLoggedInUser();
    if (this.loggedInUser.role.rank > 1) {
      Notification.displayWarning("You do not have permission to view this page, taking you back to your home page.")
      this.router.navigate(["../home"])
    }
  }

  ngOnInit(): void {
    
    this.showSpinnerEvent.emit(false)//when done loading
    this.hasLoaded = true;
    this.saving = false;
  }
  save(form: NgForm) {
    if (this.addModify == 'modify') {
      if (this.currentUser.roleId != 'FB433B41-E93D-442A-9647-476D333A1965' && this.newPassword.length>0) {
        //new user admin password
        this.currentUser.password = this.newPassword;
      }
      this.SetControlsDirty(form);
      let lc = this;
      if (form.valid) {
        this.saving = true;
        this.userService.updateAdminUser(this.currentUser).subscribe(response => {

          let dataresponse = response.apiProcessingResult;

          if (dataresponse.isError) {
            this.Notification.displayError(dataresponse.errors[0].errorMessage);
            this.saving = false;
            return;
          }
          this.Notification.displaySuccess("Admin User has been updated")
          this.showListEvent.emit(true);
          this.saving = false;
        });
      }


    } else if (this.addModify == 'add') {

      if (this.currentUser.roleId != 'FB433B41-E93D-442A-9647-476D333A1965' ) {
        //new user admin password
        this.currentUser.password = this.newPassword;
      }
      this.SetControlsDirty(form);
      if (form.valid) {
        this.saving = true;
        this.userService.addAdminUser(this.currentUser).subscribe(response => {

          let dataresponse = response.apiProcessingResult;

          if (dataresponse.isError) {
            this.Notification.displayError(dataresponse.errors[0].errorMessage);
            this.saving = false;
            return;
          }
          this.Notification.displaySuccess("Admin User has been added")
          this.refreshDataEvent.emit()
          this.showListEvent.emit(true);
          this.saving = false;
        });
      }

    }

  }
  SetControlsDirty(form: NgForm) {
    for (let eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();
    }
  }
  deleteUser() {
    this.saving = true;
    this.userService.deleteUser(this.currentUser.id).subscribe(response => {
      let dataresponse = response.apiProcessingResult;

      if (dataresponse.isError) {
        console.log(dataresponse.errors[0].errorMessage)
        this.Notification.displayError("Failed to remove user. See logs for reason.");
        this.saving = false;
        return;
      }
      this.Notification.displaySuccess("Admin User has been removed")
      this.refreshDataEvent.emit()
      this.showListEvent.emit(true);
      this.saving = false;


    })

  }
 
}
