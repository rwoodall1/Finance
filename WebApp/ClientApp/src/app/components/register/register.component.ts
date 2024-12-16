import { Component, OnInit, AfterViewInit, ViewChild,Output } from '@angular/core';
import { GlobalService, AccountService,TransActionService } from '../../services/Services';
import { NotificationService } from '../../services/notification.service'
import { AccountModel, NameLkpModel } from '../../bindingmodels/accountBindingModels';
import { TransActionCrudModel, addModidfyData } from '../../bindingmodels/transActionBindingModels';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Router } from '@angular/router';
import {  MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { NgxSpinnerService } from "ngx-spinner";
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
    standalone: false
})
export class RegisterComponent implements OnInit {
 //https://stackblitz.com/edit/angular-mat-table-pagination-example?file=src%2Fapp%2Fsimple-mat-table%2Fsimple-mat-table.component.ts
  rowData: TransActionCrudModel;
  transType: string;
  showForm: string;
  accountId: number;
  accountName: string;
  loading: boolean=true;
  registerClr: string ="aquamarine"//make dynamic

  //allRowsExpanded: boolean = false;
  data : Array<TransActionCrudModel>
  //showTable = false;

  constructor(private spinner: NgxSpinnerService,private router: Router, public Global: GlobalService, private transActionService: TransActionService, private Notification: NotificationService,) {
  
  }
 
  ngAfterViewInit() {
     this.spinner.show(); 
  }

  ngOnInit() {
    this.accountId = history.state.accountId
    
    this.getAccountTransActions();
  }
  
  
  getAccountTransActions() {
    this.loading = true
    this.showForm = '';
   this.spinner.show();
    this.transActionService.getTransActions(this.accountId,null).subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.loading = false;
        this.showForm = 'table';
        this.spinner.hide()
        return;
      }
     
      this.data = dataresponse.data;
      this.accountName = this.data[0].accountName;
      this.showForm = 'table';
      this.loading = false;
     this.spinner.hide()

    })

  }
  showEdit(data:addModidfyData) {
    this.rowData = data.data;
    this.transType = data.transType;
    this.setForm('edit')
  }
  setForm(form) {
    this.showForm = form;
   
  }


 




}

