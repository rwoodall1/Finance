import { Component, OnInit, AfterViewInit, ViewChild, Output, EventEmitter, Input,} from '@angular/core';
import { GlobalService, AccountService,TransActionService,NameService,ValidationService } from '../../services/Services';
import { NotificationService } from '../../services/notification.service'
import { AccountModel, NameLkpModel, AccountDropDown } from '../../bindingmodels/accountBindingModels';
import { TransActionCrudModel} from '../../bindingmodels/transActionBindingModels';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { DropDownNames } from '../../bindingmodels/namesBindingModels';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import {  MatPaginator } from '@angular/material/paginator';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { UntypedFormControl, ReactiveFormsModule, NgForm } from '@angular/forms';
import { NgxSpinnerService } from "ngx-spinner";
@Component({
    selector: 'app-register-Modify',
    templateUrl: './addModifyRow.component.html',
    styleUrls: ['./addModifyRow.component.css'],
    standalone: false
})
export class AddModifytRowComponent implements OnInit {
  //https://stackblitz.com/edit/angular-mat-table-pagination-example?file=src%2Fapp%2Fsimple-mat-table%2Fsimple-mat-table.component.ts

  @Input() rowData: TransActionCrudModel;
  @Input() transType: string
  @Output() setForm = new EventEmitter<string>();
  @Output() refresh = new EventEmitter();
  childData: Array<TransActionCrudModel>;
  fromDate = new UntypedFormControl();
  nameList: Array<DropDownNames>;
  accountList: Array<AccountDropDown>;
  showTheTable = false;

  constructor(private validationService: ValidationService, private spinner: NgxSpinnerService, private nameService: NameService, private accountService: AccountService, private router: Router, public Global: GlobalService, private transActionService: TransActionService, private Notification: NotificationService,) {

  }

  ngAfterViewInit() {

  }

  ngOnInit() {
    console.log(this.rowData)
    this.spinner.show();
    this.getLkpData()
    if (this.transType == 'modify') {
      this.getChildTransActions();
    } else if (this.transType == 'add') {
      this.addChildTransAction()
    }

  }

  getChildTransActions() {
    this.transActionService.getChildTransActions(this.rowData.transActionId, this.rowData.accountId).subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.spinner.hide();
        return;
      }
      this.childData = dataresponse.data;
      this.showTheTable = true;
      this.spinner.hide();
    })
  }
  getLkpData() {
    this.accountService.sysAccounts().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        return;
      }

      this.accountList = dataresponse.data;

    })
    this.nameService.getNames().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);

        return;
      }

      this.nameList = dataresponse.data;

    })
  }
  addChildTransAction() {
    if (!this.childData) {
      this.childData = new Array<TransActionCrudModel>();
    }

    let newRow = new TransActionCrudModel();
    newRow.childAccountId = this.rowData.accountId;
    newRow.payeeId = this.rowData.payeeId;
    newRow.transDate = this.rowData.transDate;
    newRow.transType = this.rowData.transType;
    newRow.refNumber = this.rowData.refNumber;
    if (this.rowData.debit>=0) {
      newRow.credit = 0;
    } else {
      newRow.debit = 0;
    }

    
    newRow.transActionId = this.rowData.transActionId;
    this.childData.push(newRow);
    this.spinner.hide();

  }
  removeChildTransAction(index) {
    let a = index;
    this.childData.splice(index, 1)

  }
  showTable() {

    this.setForm.emit('table')

  }
  save(form: NgForm) {
    this.spinner.show();

    if (form.valid) {
      this.SetControlsDirty(form)
      let validationResult = this.validationService.validateTransaction(this.rowData, this.childData);
      if (validationResult.isError) {
        this.Notification.displayError(validationResult.errors[0].errorMessage);


        this.spinner.hide();
        return;
      }
      const dataList = validationResult.data;
      if (this.transType == 'modify') {
        this.transActionService.upDateTransAction(dataList).subscribe(response => {
          const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
          if (dataresponse.isError) {
            this.Notification.displayError(dataresponse.errors[0].errorMessage);
            this.spinner.hide();
            return;
          }
          this.Notification.displaySuccess("Record Saved")
          this.refresh.emit();

          this.spinner.hide();
        })
      } else if (this.transType == 'add') {
        this.transActionService.newTransaction(dataList).subscribe(response => {
          const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
          if (dataresponse.isError) {
            this.Notification.displayError(dataresponse.errors[0].errorMessage);
            this.spinner.hide();
            return;
          }
          this.Notification.displaySuccess("Record Saved")
          this.refresh.emit();

          this.spinner.hide();
        })


      }
    }

  }

  currencyInputChanged(value) {
    var num = value.replace(/[$,]/g, "");

    return Number(num);
  }

  inBalance(): boolean {
    let retval = true;
    let _parentBalance = 0;
    let _childBalance = 0;
    // validate on form that value is number row.data.credit and debit
    //if (!decimal.TryParse(txtAmount.Text.Replace("$", ""), out _parentBalance)) {
    //  CMBox.Error("Enter a valid amount for the check.");
    //  return false;
    //}

    //SET PARENT VALUE
    if (this.rowData.credit > 0 && this.rowData.debit) {
      return false;

    } else if (this.rowData.credit > 0) {
      _parentBalance = +this.rowData.credit;
    } else if (this.rowData.debit > 0) {
      _parentBalance = +this.rowData.debit

    }

    this.childData.forEach(item => {

      _childBalance += ((item.credit == null ? 0 : +item.credit) + (item.debit == null ? 0 : +item.debit))

    })
    //foreach(TransActionCrudModel item in bsChildTransActions)
    //{
    //  _childBalance += item.Credit == null ? 0 : item.Credit;//it is a check so always credit in the details

    //}
    if (_childBalance != _parentBalance) {

      return false;
    }
    return retval;
  }
  deleteTransaction() {
    this.spinner.show();
    this.transActionService.deleteTransAction(this.rowData.transActionId).subscribe(response => {

      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.spinner.hide();
        return;
      }
      this.refresh.emit();

      this.spinner.hide();
    })


  }
  SetControlsDirty(form: NgForm) {
    for (let eachControl in form.controls) {
      (<UntypedFormControl>form.controls[eachControl]).markAsDirty();

    }
  }
  checkAccountId(index,  form: NgForm) {
    
      let ctrl = (<UntypedFormControl>form.controls['childAccount' + index])
    if (ctrl && ctrl.dirty ) {
        if (!this.childData[index].accountId || this.childData[index].accountId < 1) {
          return true
        } else { return false }
      } else { return false }
     
  }
  
  checkChildAmount(form: NgForm, index) {
    let retval = false;
    let ctrlname = 'childAmount' + index;

    let ctrl = (<UntypedFormControl>form.controls[ctrlname])
    console.log(ctrl)
    if (!ctrl) {
      retval = true;
      return true
    }
    if (ctrl.errors?.required) {
      retval = true;
      return true
    }
    if (!ctrl.value || ctrl.value == ''|| ctrl.value=='$0.00') {
      retval = true;
      return true
    }
    return retval;
   
  }
}
