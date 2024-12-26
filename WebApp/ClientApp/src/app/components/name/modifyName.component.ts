import { Component,  } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService, NameService,NodeService } from '../../services/Services';

import { NotificationService } from '../../services/notification.service'
import { CompleteName} from '../../bindingmodels/nameBindingModels';
import { LoggedInUser } from '../../bindingmodels/userBindingModel';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Observable, of } from 'rxjs';
import {  MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TreeNode } from 'primeng/api';
import { NgForm, UntypedFormControl } from '@angular/forms';



@Component({
    selector: 'app-modifyName',
    templateUrl: './modifyName.component.html',
    styleUrls: ['./modifyName.component.css'],
    standalone: false
})
export class ModifyNameComponent {


  user: LoggedInUser;
  otherName: CompleteName;
  loaded: boolean;
  saving: boolean = false;
  addModify: string;
  

  constructor(private nodeService: NodeService,private nameService:NameService,public dialog: MatDialog, private router: Router,public Global: GlobalService, private Notification: NotificationService, ) {
    this.user = this.Global.getLoggedInUser();
  
  }
  ngOnInit() {
    this.addModify = history.state.addModify;
    let id = history.state.id;
    this.addModifyName(id);
    
  }
 
  addModifyName(id) {
    if (this.addModify == 'Modify') {
      this.nameService.getName(id).subscribe(response => {
        const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;

        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);
         
          return;
        }
        this.otherName = dataresponse.data;
       
      
        this.loaded = true;
      })
    } else {
      this.otherName = new CompleteName();
      
      this.loaded = true;
    }
  }
  
  save(form:NgForm) {
    //this.SetControlsDirty(form);
    //if (form.valid) {
    //  this.saving = true;
    //  this.accountService.saveAccount(this.account).subscribe(response => {
    //    const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;
    //    if (dataresponse.isError) {
    //      this.Notification.displayError(dataresponse.errors[0].errorMessage);
    //      this.saving = false;
    //      return;
    //    }
    //    this.saving = false;
    //    this.Notification.displaySuccess("Account Saved")
    //    this.router.navigateByUrl("/chartOfAccounts")

    //  })
    //}

  }
  SetControlsDirty(form: NgForm) {
    for (let eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();
      (<UntypedFormControl>form.controls[eachControl]).markAsTouched();
    }
  }
 
  cancel() {

    this.router.navigateByUrl('/chartOfAccounts')

  }
  deleteAccount() {


  }
}
