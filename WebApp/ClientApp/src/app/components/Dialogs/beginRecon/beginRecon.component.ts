import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalService,AccountService,  } from '../../../services/Services';
import { ApiProcessingResult } from '../../../bindingmodels/coreBindingModel';
import { Balances, ReconciledData } from '../../../bindingmodels/transActionBindingModels';
import { NotificationService } from '../../../services/notification.service'
import { AccountModel, AccountDropDown  } from '../../../bindingmodels/accountBindingModels';
import {  MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from "ngx-spinner";
import { TransActionService } from '../../../services/Services';
import { ZeroValidator, AccountValidator } from '../../../directives/validators'
import * as moment from 'moment'
import { UntypedFormControl, NgForm } from '@angular/forms';
@Component({
    selector: 'app-beginRecon-componet',
    templateUrl: './beginRecon.component.html',
    styleUrls: ['./beginRecon.component.css'],
    standalone: false
})
export class BeginReconComponent implements OnInit{
  accountId: number
  balances: Balances;
  loaded = false;
  accountList: Array<AccountDropDown>;
  reconcileInformation = new ReconciledData();
  servicChargeAmt: number;
  serviceDate: Date;
  serviceAccount: number=0;
  interestCharge: number=0;
  interestDate: Date;
  interestAccount: number;
  constructor(private transActionService: TransActionService, @Inject(MAT_DIALOG_DATA) public data: any, private spinner: NgxSpinnerService, private dialogRef: MatDialogRef<BeginReconComponent>,private accountService:AccountService,private router: Router, public Global: GlobalService, private Notification: NotificationService) {

    this.reconcileInformation=data.data
      this.accountId = data.data.accountId;
 
  
  }
  ngOnInit() {
   
    this.getAccounts();
    if (!this.reconcileInformation.reconciledBalance) {
      this.spinner.show()
      this.getLastStatementBalance();

    } else {

      this.loaded = true;
      
    }
    
  }
  insertUpdateReconciliation(form: NgForm) {
 this.SetControlsDirty(form);
    if (form.valid) {
      this.transActionService.insertUpdateReconciliation(this.reconcileInformation).subscribe(response => {
        const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;
        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);

          return;
        }
        this.reconcileInformation.id=dataresponse.data;
        this.continue();

      })
    }

  }
  getLastStatementBalance() {
    this.transActionService.getLastStatement(this.accountId).subscribe(response => {
      const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.spinner.hide
        return;
      }
      this.balances = dataresponse.data;
      this.setReconciliationData();
   
      
  
      this.loaded = true;
      this.spinner.hide();
    })

  }
  setReconciliationData() {
    this.reconcileInformation.accountId = this.accountId;
    this.reconcileInformation.previousStatementDate = this.balances.previousStatementDate;
    this.reconcileInformation.statementDate = moment(this.balances.previousStatementDate).add(30, 'days').toDate();
   

    this.reconcileInformation.endingBalance = this.balances.reconciled ? 0 : this.balances.endingBalance;
    this.reconcileInformation.id =this.balances.reconciled?0:  this.balances.id;
 this.reconcileInformation.reconciledBalance = this.balances.reconciled ?this.balances.endingBalance :this.balances.reconciledBalance;
    this.reconcileInformation.seviceCharge = this.servicChargeAmt ? this.servicChargeAmt : 0;
    this.reconcileInformation.interestCharge = this.interestCharge ? this.interestCharge : 0;
    this.reconcileInformation.serviceChargeDate = this.serviceDate;
    this.reconcileInformation.serviceChargeAccount = this.serviceAccount
    this.reconcileInformation.intererestAccount = this.interestAccount
    this.reconcileInformation.interestDate = this.interestDate;
  

  }
  getAccounts() {
    this.accountService.sysAccounts().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        return;
      }

      this.accountList = dataresponse.data;

    })
  }
  currencyInputChanged(value) {
    var num = value.replace(/[$,]/g, "");

    return Number(num);
  }
  SetControlsDirty(form: NgForm) {
    for (let eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();

    }
  }
  continue() {


  
      this.dialogRef.close(this.reconcileInformation)
    
  }
  cancel() {
 this.router.navigateByUrl('/')
  }
}
