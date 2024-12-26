import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService, AccountService,NodeService, NameService } from '../../services/Services';

import { NotificationService } from '../../services/notification.service'
import { AccountModel,ChartOfAccounts} from '../../bindingmodels/accountBindingModels';
import { LoggedInUser } from '../../bindingmodels/userBindingModel';
import { CompleteName } from '../../bindingmodels/nameBindingModels';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Observable, of } from 'rxjs';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TreeNode } from 'primeng/api';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';



@Component({
    selector: 'app-name',
    templateUrl: './name.component.html',
    styleUrls: ['./name.component.css'],
    standalone: false
})
export class NameComponent {


  user: LoggedInUser;
  otherNames: Array<CompleteName>;
  dataSource1: MatTableDataSource<CompleteName>;
  loaded: boolean;
  columnsToDisplay = ['status','name','email','phone','mobilePhone'];
 
  constructor(private nodeService: NodeService,private nameService:NameService,public dialog: MatDialog, private router: Router,public Global: GlobalService, private Notification: NotificationService, ) {
    this.user = this.Global.getLoggedInUser();
  
  }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageSizes = [];
  ngOnInit() {

    var a = this.paginator;
    this.getOtherNames();
  }
  ngAfterViewInit() {

  }

  getOtherNames() {
    this.nameService.getOtherNames().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;

      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.loaded = true;
        return;
      }
      this.loaded = true;
      this.otherNames = dataresponse.data;
      this.dataSource1 = new MatTableDataSource<CompleteName>(this.otherNames);
      this.dataSource1.paginator = this.paginator;
      
    })

  }
 
  editName(id) {
    this.router.navigateByUrl('name/modify', { state: { addModify: 'Modify', id: id } });
   // this.router.navigate()
    
  }
  addName() {
    this.router.navigateByUrl('name/add', { state: { addModify: 'Add', id: null} });

  }
}
