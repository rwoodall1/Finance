import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalService, AccountService,NodeService } from '../../services/Services';

import { NotificationService } from '../../services/notification.service'
import { AccountModel,ChartOfAccounts} from '../../bindingmodels/accountBindingModels';
import { LoggedInUser } from '../../bindingmodels/userBindingModel';
import { ApiProcessingResult } from '../../bindingmodels/coreBindingModel';
import { Observable, of } from 'rxjs';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { TreeNode } from 'primeng/api';



@Component({
    selector: 'app-chartofaccount',
    templateUrl: './chartOfAccount.component.html',
    styleUrls: ['./chartOfAccount.component.css'],
    standalone: false
})
export class ChartOfAccountComponent {


  user: LoggedInUser;
  accounts: Array<TreeNode>;

  loaded: boolean;
  dataTree: ChartOfAccounts[];
  files: TreeNode[];
 
  constructor(private nodeService: NodeService,private accountService:AccountService,public dialog: MatDialog, private router: Router,public Global: GlobalService, private Notification: NotificationService, ) {
    this.user = this.Global.getLoggedInUser();
  
  }
  ngOnInit() {
    
    
    this.getAllAccounts();
  }

  getAllAccounts() {
    this.accountService.getAllAccounts().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;

      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.loaded = true;
        return;
      }
     // this.accounts = dataresponse.data;
      this.dataTree = dataresponse.data;
      this.exapandORcollapse(this.dataTree)
      console.log(this.dataTree)
     
      this.loaded = true;
    })

  }
  exapandORcollapse(nodes) {
    for (let node of nodes) {
      if (node.children) {
        if (node.expanded == true)
          node.expanded = false;
        else
          node.expanded = true;
        for (let cn of node.children) {
          this.exapandORcollapse(node.children);
        }
      }
    }
  }
  editAccount(accountid) {
    this.router.navigateByUrl('chartOfAccounts/modify', { state: { addModify: 'Modify', id: accountid } });
   // this.router.navigate()
    
  }
  addAccount() {
    this.router.navigateByUrl('chartOfAccounts/add', { state: { addModify: 'Add', id: null} });

  }
}
