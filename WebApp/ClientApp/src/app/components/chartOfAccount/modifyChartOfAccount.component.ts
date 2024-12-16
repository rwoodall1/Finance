import { Component,  } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService, AccountService,NodeService } from '../../services/Services';

import { NotificationService } from '../../services/notification.service'
import { AccountModel,ChartOfAccounts,AccountType,SubAccount} from '../../bindingmodels/accountBindingModels';
import { LoggedInUser } from '../../bindingmodels/userBindingModel';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Observable, of } from 'rxjs';
import {  MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TreeNode } from 'primeng/api';
import { NgForm, UntypedFormControl } from '@angular/forms';



@Component({
    selector: 'app-modifyChartofaccount',
    templateUrl: './modifyChartOfAccount.component.html',
    styleUrls: ['./modifyChartOfAccount.component.css'],
    standalone: false
})
export class ModifyChartOfAccountComponent {


  user: LoggedInUser;
  accounts: Array<TreeNode>;
  subAccounts: Array<SubAccount>;
  loaded: boolean;
  saving: boolean = false;
  addModify: string;
  account: AccountModel;
  accountTypes: Array<AccountType>;

  constructor(private nodeService: NodeService,private accountService:AccountService,public dialog: MatDialog, private router: Router,public Global: GlobalService, private Notification: NotificationService, ) {
    this.user = this.Global.getLoggedInUser();
  
  }
  ngOnInit() {
    this.addModify = history.state.addModify;
    let id = history.state.id;
    this.addModifyAccount(id);
    
  }
  getSubAccounts(type:string) {
    this.accountService.getSubAccounts(type).subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<SubAccount>> = response.apiProcessingResult;

      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.loaded = true;
        return;
      }
      this.subAccounts = dataresponse.data;
     
    
    })

  }
  addModifyAccount(accountId) {
    if (this.addModify == 'Modify') {
      this.accountService.getAccount(accountId).subscribe(response => {
        const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;

        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);
          this.loaded = true;
          return;
        }
        this.account = dataresponse.data;
       
       this.getAccountTypes()
        this.getSubAccounts(this.account.name);
        this.loaded = true;
      })
    } else {
      this.account = new AccountModel();
      this.getAccountTypes()
      this.loaded = true;
    }
  }
  getAccountTypes() {

    this.accountService.getAccountTypes().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<AccountType>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
       
        return;
      }
      this.accountTypes = dataresponse.data;
      console.log(this.accountTypes)
    });

  }
  save(form:NgForm) {
    this.SetControlsDirty(form);
    if (form.valid) {
      this.saving = true;
      this.accountService.saveAccount(this.account).subscribe(response => {
        const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;
        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);
          this.saving = false;
          return;
        }
        this.saving = false;
        this.Notification.displaySuccess("Account Saved")
        this.router.navigateByUrl("/chartOfAccounts")

      })
    }

  }
  SetControlsDirty(form: NgForm) {
    for (let eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();
      (<UntypedFormControl>form.controls[eachControl]).markAsTouched();
    }
  }
  setTypeName(event:any) {

    this.account.accountType = this.accountTypes.find(x=>x.id===event.value).name;;

  }
  cancel() {

    this.router.navigateByUrl('/chartOfAccounts')

  }
  deleteAccount() {


  }
}
