import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService, TransActionService } from '../../services/Services';

import { NotificationService } from '../../services/notification.service'
import { LoggedInUser } from '../../bindingmodels/userBindingModel';
import {MatTableDataSource } from '@angular/material/table';
import * as moment from 'moment';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Observable, of } from 'rxjs';
import {  MatDialog,  MatDialogConfig } from '@angular/material/dialog';
import { NgxSpinnerService } from "ngx-spinner";
import { TransActionCrudModel, addModidfyData } from '../../bindingmodels/transActionBindingModels';
import { FunctionBalances } from '../../bindingmodels/miscBindingModels';
import { BeginReconComponent } from '../Dialogs/beginRecon/beginRecon.component';
import {  ReconciledData } from '../../bindingmodels/transActionBindingModels';

@Component({
    selector: 'app-reconcile',
    templateUrl: './reconcile.component.html',
    styleUrls: ['./reconcile.component.css'],
    standalone: false
})
export class ReconcileComponent {
  balances = new FunctionBalances();
  accountId: number;
  moment = moment;
  transActions: Array<TransActionCrudModel>;
  debits: Array<TransActionCrudModel> = new Array<TransActionCrudModel>();
  credits: Array<TransActionCrudModel> = new Array<TransActionCrudModel>();
  reconcileInformation = new ReconciledData();
  dataSource1: any;
  dataSource2: any;
  columnsToDisplay = ['clr', 'date', 'number', 'payee', 'debit',];
  selectAllChecks = false;
  selectAllDeposites = false;

  constructor(private transActionService: TransActionService, private spinner: NgxSpinnerService, public dialog: MatDialog, private router: Router, public Global: GlobalService, private Notification: NotificationService,) {

  }
  ngOnInit() {
    this.accountId = history.state.accountId

    this.getUnReconciledTransActions();

  }
  getUnReconciledTransActions() {
    this.spinner.show();
    this.transActionService.getTransActions(this.accountId, 'Yes').subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        this.spinner.hide()
        return;
      }
      this.transActions = dataresponse.data;

      this.transActions.forEach(row => {
        if (row.debit > 0) {
          this.debits.push(row)
        } else if (row.credit > 0) {
          this.credits.push(row)
        }

      }

      )


      this.dataSource1 = new MatTableDataSource<TransActionCrudModel>(this.debits);

      this.dataSource2 = new MatTableDataSource<TransActionCrudModel>(this.credits);
      console.log(this.credits)
      console.log(this.debits)
      this.spinner.hide()
      
        this.setBalances();
      
    })

  }

  setBalances() {
    this.reconcileInformation.accountId = this.accountId;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = "650px",
      dialogConfig.height = "400px",
      dialogConfig.closeOnNavigation = true,
      dialogConfig.data = { data: this.reconcileInformation }
    const dialogRef = this.dialog.open(BeginReconComponent, dialogConfig)
    dialogRef.afterClosed().subscribe(result => {
      this.reconcileInformation = result;
      this.calculateBalances();
    })

  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows(grid, checkBox) {
    if (grid == 'checks') {
      this.debits.forEach(row => {
        row.clr = this.selectAllChecks
      })
    } else if (grid == 'deposites') {
      this.credits.forEach(row1 => {
        row1.clr = this.selectAllDeposites
      })

    }
    this.calculateBalances();
  }

  calculateBalances() {
    let clrPayment: number = 0
    let clrPaymentAmount: number = 0
    this.debits.forEach(row => {
      if (row.clr == true) {
        clrPayment += 1;
        clrPaymentAmount += row.debit;
      }
    })
    this.balances.checksCleared = clrPayment;
    this.balances.checksClearedAmt = clrPaymentAmount;
    let clrDeposites: number = 0
    let clrDepositeAmt: number = 0
    this.credits.forEach(row => {
      if (row.clr == true) {
        clrDeposites += 1;
        clrDepositeAmt += row.credit;
      }

    })

    this.balances.depositesCleared = clrDeposites; //+ this.reconcileInformation.interestCharge;
    this.balances.depositesAmt = clrDepositeAmt;
    this.balances.clearedBalance = (this.reconcileInformation.reconciledBalance + this.balances.depositesAmt + Number(this.reconcileInformation.interestCharge)) - (this.balances.checksClearedAmt + Number(this.reconcileInformation.seviceCharge));
    this.balances.difference = this.reconcileInformation.endingBalance - this.balances.clearedBalance;
  }
  setCleared() {
    let data = this.debits;
    this.credits.forEach(row => {
      data.push(row);
    });
    this.insertUpdateReconciliation();
    this.spinner.show();
    this.transActionService.setCleared(data).subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        this.spinner.hide()
        return;
      }
      this.spinner.hide()
      this.router.navigate(['/home']);

    })
  }
  insertUpdateReconciliation() {
    this.transActionService.insertUpdateReconciliation(this.reconcileInformation).subscribe(response => {
      const dataresponse: ApiProcessingResult<any> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        return;
      }


    })


  }
  setReconciled() {
    if (this.balances.difference != 0) {
      this.Notification.displayError("The difference is not zero, please clear all transactions or adjust the reconciled balance")
      return;
    }
    this.spinner.show()
    this.reconcileInformation.reconciled = true;
    this.insertUpdateReconciliation();
    this.transActionService.insertUpdateReconciliation(this.reconcileInformation).subscribe(response => {
      const dataresponse: ApiProcessingResult<number> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        this.spinner.hide()
        return;
      }
      let reconciliationId = dataresponse.data;
      if (reconciliationId < 1) {
        this.Notification.displayError("Failed to get reconcile Id")
        return
      }
      let reconceledTransActions = new Array<TransActionCrudModel>();
      this.debits.forEach(row => {
        if (row.clr == true) {
          row.reconciledId = reconciliationId;
          row.reconciled = true;
          reconceledTransActions.push(row);
        }
      })
      this.credits.forEach(row => {
        if (row.clr == true) {
          row.reconciledId = reconciliationId;
          row.reconciled = true;
          reconceledTransActions.push(row);
        }
      });
          this.transActionService.setReconciled(reconceledTransActions).subscribe(response => {
            const dataresponse2: ApiProcessingResult<any> = response.apiProcessingResult;
            if (dataresponse.isError) {
              this.Notification.displayError(dataresponse.errors[0].errorMessage);

              this.spinner.hide()
              return;
            }
            this.Notification.displaySuccess("Reconciliation completed successfully")
            
          });

        })

    }
}


