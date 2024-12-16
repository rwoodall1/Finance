import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../../components/modules/shared.module';
import { ReconcileRoutingModule } from './reconcile.routing';
import { ModalModule } from '../../shared/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TreeTableModule } from 'primeng/treetable';
import { ReconcileComponent } from '../../components/reconcile/reconcile.component'

@NgModule({
  declarations: [
    ReconcileComponent,
  
  ],
  imports: [
    CommonModule,
    ModalModule,
    FormsModule,
    ReactiveFormsModule,
   ReconcileRoutingModule,
    SharedModule,
    TreeTableModule,
  ]
})
export class ReconcileModule { }
