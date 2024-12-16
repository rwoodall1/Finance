import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../../components/modules/shared.module';
import { ChartOfAccountsRoutingModule } from './chartofaccounts.routing';
import { ModalModule } from '../../shared/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TreeTableModule } from 'primeng/treetable';
import { ChartOfAccountComponent } from '../../components/chartOfAccount/chartOfAccount.component'
import { ModifyChartOfAccountComponent } from '../../components/chartOfAccount/modifyChartOfAccount.component';
@NgModule({
  declarations: [
    ChartOfAccountComponent,
    ModifyChartOfAccountComponent
  ],
  imports: [
    CommonModule,
    ModalModule,
    FormsModule,
    ReactiveFormsModule,
    ChartOfAccountsRoutingModule,
    SharedModule,
    TreeTableModule,
  ]
})
export class ChartOfAccountsModule { }
