import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BankFeedRoutingModule } from '../../components/modules/bankFeed.routing';
import { SharedModule } from '../../components/modules/shared.module';

import { BankFeedComponent } from '../../components/bankFeed/bankFeed.component';







@NgModule({
  declarations: [
    BankFeedComponent,
 
  ],
  imports: [
   
    CommonModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    BankFeedRoutingModule,

  ],
  exports: []
})
export class BankFeedModule { }
