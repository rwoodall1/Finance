import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../../components/modules/shared.module';
import { NameRoutingModule } from './name.routing';
import { ModalModule } from '../../shared/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TreeTableModule } from 'primeng/treetable';
import { NameComponent } from '../../components/name/name.component'
import { ModifyNameComponent } from '../../components/name/modifyName.component';
@NgModule({
  declarations: [
    NameComponent,
    ModifyNameComponent
  ],
  imports: [
    CommonModule,
    ModalModule,
    FormsModule,
    ReactiveFormsModule,
    NameRoutingModule,
    SharedModule,
    TreeTableModule,
  ]
})
export class NameModule { }
