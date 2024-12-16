import { Component, OnInit, AfterViewInit, ViewChild, Output, ElementRef } from '@angular/core';
import { GlobalService, BankFeedService, AccountService, NameService, } from '../../services/Services';
import { NotificationService } from '../../services/notification.service'
import { BankFeedModel } from '../../bindingmodels/bankFeedModels';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Router } from '@angular/router';
import {  MatTableDataSource } from '@angular/material/table';
import {MatPaginator } from '@angular/material/paginator';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { NgxSpinnerService } from "ngx-spinner";

import { DropDownNames } from '../../bindingmodels/namesBindingModels';
import { AccountModel, NameLkpModel, AccountDropDown } from '../../bindingmodels/accountBindingModels';
@Component({
    selector: 'app-bankFeed',
    templateUrl: './bankFeed.component.html',
    styleUrls: ['./bankFeed.component.css'],
    standalone: false
})

  //was using tabs but found it loaded mat table too slow. Here was a solution but deciced to not us tabs. Look at this if I change back to tabs
  //https://www.youtube.com/watch?v=3Cw69t41e-k

export class BankFeedComponent implements OnInit {
  //https://stackblitz.com/edit/angular-mat-table-pagination-example?file=src%2Fapp%2Fsimple-mat-table%2Fsimple-mat-table.component.ts
 
  dataSource1: MatTableDataSource<BankFeedModel>;
  data: Array<BankFeedModel>;
  nameList: Array<DropDownNames>;
  accountList: Array<AccountDropDown>;
  selectedAction: string;
  currentBankId: string ;
  banks: any;
  file: File = null;

  columnsToDisplay = ['import', 'status', 'type', 'date', 'chk', 'downloadname', 'name', 'account', 'debit', 'credit'];
  constructor(private bankFeedService: BankFeedService, private nameService: NameService, private accountService:AccountService, private spinner: NgxSpinnerService, private router: Router, public Global: GlobalService, private Notification: NotificationService,) {
    
  }
  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageSizes = [];
  ngOnInit() {
  
    this.getLkpData();
    this.getBanks();
    //this.setDataSource()


  }
  ngAfterViewInit() {
    //this.dataSourceWithPageSize.paginator = this.paginatorPageSize;
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
  onChange() {
    this.setDataSource();
  }
  setDataSource() {
    this.spinner.show();
    this.bankFeedService.getImportedData(this.currentBankId).subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<BankFeedModel>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.spinner.hide();
        return;
      }

      this.data = dataresponse.data;
      //this.pageSizes = [15, 20, 25, this.data.length];
      this.dataSource1 = new MatTableDataSource<BankFeedModel>(this.data);
      this.dataSource1.paginator = this.paginator;
      this.spinner.hide();
    })
  }
 
  importTransAction() {
    if (this.selectedAction == 'Add/Approved') {
      this.selectedAction = ""
      //Import Data
      this.bankFeedService.importData(this.dataSource1.filteredData).subscribe(response => {


      })
    } else if (this.selectedAction == 'Ignore') {
      this.selectedAction = ""
      //Set Imported
    }

  }
  
  getBanks() {
    this.accountService.getBankAccounts().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.spinner.hide
        return;
      }

      this.banks = dataresponse.data;
    });
  }
  onFilechange(event: any) {

    const file: File = event.target.files[0];
    if (file) {

      this.file = file;
    }

  }
  upload() {
    if (this.file) {
      const formData = new FormData();

      formData.append('File', this.file, this.file.name);
      formData.append('AccountId', '1234')

      this.bankFeedService.readBankFeed(formData).subscribe(response => {
        let dataresponse = response.apiProcessingResult;

        if (dataresponse.isError) {
          this.Notification.displayError(dataresponse.errors[0].errorMessage);

          return;
        }
        this.data = dataresponse.data;
       
      })
    }
  }
}

 






