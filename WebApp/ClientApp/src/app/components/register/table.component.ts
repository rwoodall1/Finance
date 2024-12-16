import { Component, OnInit, AfterViewInit, ViewChild,Input, EventEmitter,Output } from '@angular/core';
import { GlobalService, AccountService,TransActionService } from '../../services/Services';
import { NotificationService } from '../../services/notification.service'
import { AccountModel, NameLkpModel } from '../../bindingmodels/accountBindingModels';
import { TransActionCrudModel,addModidfyData} from '../../bindingmodels/transActionBindingModels';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import {  MatPaginator } from '@angular/material/paginator';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { NgxSpinnerService } from "ngx-spinner";
import * as moment from 'moment';
@Component({
    selector: 'app-register-table',
    templateUrl: './table.component.html',
    styleUrls: ['./table.component.css'],
    standalone: false
})
export class TableComponent implements OnInit {
 //https://stackblitz.com/edit/angular-mat-table-pagination-example?file=src%2Fapp%2Fsimple-mat-table%2Fsimple-mat-table.component.ts

  @Input() data: Array<TransActionCrudModel>
  @Output() showEdit = new EventEmitter<addModidfyData>();
  registerClr: string ="aquamarine"//make dynamic
  ht = "300px"
  selected = '';
  allRowsExpanded: boolean = false;
  //data : Array<TransActionCrudModel>
 
   columnsToDisplay = ['date', 'number',  'payee', 'debit',  'clr',  'credit', 'balance'];

  //columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
  expandedElement: TransActionCrudModel | null;
  dataSource: any;
  dataSourceWithPageSize: any;
  loaded = false
  showFirstLastButtons = true;
  constructor(private spinner: NgxSpinnerService,private router: Router, public Global: GlobalService, private transActionService: TransActionService, private Notification: NotificationService,) {
  
  }
 // @ViewChild('paginator') paginator: MatPaginator;
  @ViewChild('paginatorPageSize') paginatorPageSize: MatPaginator;

  pageSizes = [];
 
  ngAfterViewInit() {
  
 
    this.dataSourceWithPageSize.paginator = this.paginatorPageSize;
   
  }

  ngOnInit() {
    this.pageSizes = [15,20,25, this.data.length];
    this.loaded = true
  
    this.dataSourceWithPageSize = new MatTableDataSource<TransActionCrudModel>(this.data);
    this.spinner.hide()
  }
  edit(event: MouseEvent,data) {
    //event.preventDefault(); for right click only
   const rowdata = new addModidfyData(data,'modify')
    this.showEdit.emit(rowdata);

  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSourceWithPageSize.filter = filterValue.trim().toLowerCase();
  }
  addTransaction() {
    if (this.selected) {


      let transDate = moment().toDate();
      //return;
      const item = new TransActionCrudModel();

      switch (this.selected) {
        case 'CHK':

          item.accountId = this.data[0].accountId,
            item.childAccountId = 0;
          item.payeeId = 0;

          item.transDate = transDate;
          item.transType = 'CHK';
          item.refNumber = '';
          item.debit = 0
          item.credit = null;
          item.transActionId = 0


          break;
        case 'TRANSF':


          item.accountId = this.data[0].accountId,
            item.childAccountId = 0;
          item.payeeId = 0;
          item.transDate = transDate;
          item.transType = 'TRANSF';
          item.refNumber = '';
          item.debit == 0
          item.credit = null;
          item.transActionId = 0

          break;
        case 'DEP':


          item.accountId = this.data[0].accountId,
           item.childAccountId = 0;
          item.payeeId = 0;
          item.transDate = transDate;
          item.transType = 'DEP';
          item.refNumber = '';
          item.debit == null;
          item.credit = 0;
          item.transActionId = 0

          break;

      }
      const rowdata = new addModidfyData(item, 'add')
      this.showEdit.emit(rowdata);
    }
    
  }
 
}
