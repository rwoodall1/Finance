import { Component, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalService,AccountService,  } from '../../../services/Services';
import { ApiProcessingResult } from '../../../bindingmodels/coreBindingModel';
import { NotificationService } from '../../../services/notification.service'
import { AccountModel,  } from '../../../bindingmodels/accountBindingModels';
import {  MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from "ngx-spinner";
@Component({
    selector: 'app-get-register-componet',
    templateUrl: './get-register-componet.component.html',
    styleUrls: ['./get-register-componet.component.css'],
    standalone: false
})
export class GetRegisterComponetComponent implements OnInit{
  bankAccounts: Array<AccountModel>;
  whichForm: string;
  constructor(@Inject(MAT_DIALOG_DATA) public data: any,private spinner: NgxSpinnerService,private dialogRef: MatDialogRef<GetRegisterComponetComponent>,private accountService:AccountService,private router: Router, public Global: GlobalService, private Notification: NotificationService) {

    this.whichForm = data.type;
  }
  ngOnInit() {
    this.spinner.show()
    this.getBankAccounts();

  }
  getBankAccounts() {
    this.accountService.getBankAccounts().subscribe(response => {
      const dataresponse: ApiProcessingResult<Array<any>> = response.apiProcessingResult;
      if (dataresponse.isError) {
        this.Notification.displayError(dataresponse.errors[0].errorMessage);
        this.spinner.hide
        return;
      }
      // this.accounts = dataresponse.data;
      this.bankAccounts = dataresponse.data;
      this.spinner.hide();
    })

  }
  openForm(accountid) {
    if (this.whichForm == 'register') {
      this.router.navigate(['register'], { state: { accountId: accountid } })
      this.dialogRef.close();
    } else if (this.whichForm == 'reconcile') {

      this.router.navigate(['reconcile'], { state: { accountId: accountid } })
      this.dialogRef.close();
    }
    
  }
}
