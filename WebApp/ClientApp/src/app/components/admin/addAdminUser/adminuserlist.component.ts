import { Component, ViewEncapsulation, ViewChild, Output, Input, OnInit, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService } from '../../../services/Services';
import { NotificationService } from '../../../services/notification.service'
import { UserService } from '../../../services/Services'
import { LoggedInUser } from '../../../bindingmodels/userBindingModel';
import { ApiProcessingResult } from '../../../bindingmodels/coreBindingModel';
import { UserOfList,AdminUser } from '../../../bindingmodels/userBindingModel';
@Component({
    selector: 'app-adminuserlist',
    templateUrl: './adminuserlist.component.html',
    styleUrls: ['./adminuserlist.component.css'],
    standalone: false
})
export class AdminUserListComponent implements OnInit {
  loggedInUser: LoggedInUser;
  @Output() showSpinnerEvent = new EventEmitter<boolean>();
  @Output() refreshDataEvent = new EventEmitter<boolean>();
  users: Array<AdminUser>;
  displayedUsers: Array<AdminUser>;
  showSpinner = true;
  nameascending = true;
  emailascending = true;
  showlist: boolean = true;
  addModify: string;
  currentUser: AdminUser;
  searchType: string = "Name"
 searchValue:string=' '
  constructor(private userService: UserService, private router: Router, public Global: GlobalService, private Notification: NotificationService) {
    this.loggedInUser = this.Global.getLoggedInUser();
    if (this.loggedInUser.role.rank > 1) {
      Notification.displayWarning("You do not have permission to view this page, taking you back to your home page.")
      this.router.navigate(["../home"])
    }
    this.getAdminUsers();

  }

  ngOnInit(): void {
    let a = 1;
  }
  search(searchVal) {

    let ln = this.searchValue.trim().length;
    let seachList=this.users
    switch (this.searchType) {
      case 'Name':
        if (ln > 0) {
          let filteredUsers = seachList.filter(a => a.lastName.substr(0, ln).toUpperCase() == this.searchValue.trim().toUpperCase());
          if (filteredUsers.length > 0) {
            this.displayedUsers = filteredUsers;
          }

        } else {
          this.displayedUsers = this.users;

        }
        break;

      case 'Email':
        if (ln > 0) {
          let filteredUsers = seachList.filter(a => a.emailAddress.substr(0, ln).toUpperCase() == this.searchValue.trim().toUpperCase());
          if (filteredUsers.length > 0) {
            this.displayedUsers = filteredUsers;
          }

        } else {
          this.displayedUsers = this.users;

        }
        break;

    }

  
  
    
  }
  addModifyUser(addModifyVal,user) {

    this.addModify = addModifyVal;
    if (this.addModify == 'add') {
      this.currentUser = new AdminUser();
    } else {

        this.currentUser = user;
    }
 
    this.showlist=false

  }
 
  private getAdminUsers() {
    this.userService
      .getAdminUsers().subscribe(response => {
        const dataresponse: ApiProcessingResult<Array<AdminUser>> = response.apiProcessingResult;
        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);
          this.showSpinner = false;
          return;
        }
        this.users = dataresponse.data;
        this.displayedUsers = this.users;
        this.showSpinner = false;
      });

  }
  sortOrder(field: string) {
    switch (field) {
      case "name": {
        if (this.nameascending) {
          this.displayedUsers.sort((a, b) => a.lastName.toUpperCase().localeCompare(b.lastName.toUpperCase()));
          this.nameascending = false;
        } else {
          this.displayedUsers.sort((a, b) => b.lastName.toUpperCase().localeCompare(a.lastName.toUpperCase()));
          this.nameascending = true;
        }
        break;
      }

      case "email": {
        if (this.emailascending) {
          this.displayedUsers.sort((a, b) => a.emailAddress.toUpperCase().localeCompare(b.emailAddress.toUpperCase()));
          this.emailascending = false;
        } else {
          this.displayedUsers.sort((a, b) => b.emailAddress.toUpperCase().localeCompare(a.emailAddress.toUpperCase()));
          this.emailascending = true;
        }
        break;
      }

      

    }
  }

}
