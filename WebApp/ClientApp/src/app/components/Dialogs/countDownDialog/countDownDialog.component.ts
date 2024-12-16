import { Component, Inject, OnInit, OnDestroy, ViewEncapsulation,} from '@angular/core';
import {  MAT_DIALOG_DATA,  MatDialog,  MatDialogRef } from '@angular/material/dialog';
import { AuthService } from '../../../services/Services';
import { NotificationService } from '../../../services/notification.service'
import { LoggedInUser } from '../../../bindingmodels/userBindingModel';
import { CountDownComponent } from '../../../components/countDown/count-down.component';
@Component({
    selector: 'app-countDown',
    templateUrl: './countDownDialog.component.html',
    styleUrls: ['./countDownDialog.component.css'],
    standalone: false
})
export class CountDownDialogComponent implements OnInit {
  
  seconds: number;
  constructor(private auth:AuthService, private Notification: NotificationService, public dialogRef: MatDialogRef<CountDownDialogComponent>, @Inject(MAT_DIALOG_DATA) data) {
  
  }

  ngOnInit(): void {
    
    
    
  }
 
  close() {
 
    this.dialogRef.close();
  }


  
 
}
