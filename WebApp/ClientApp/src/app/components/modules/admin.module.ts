import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminModuleRoutingModule } from '../../components/modules/admin.routing';
import { SharedModule } from '../../components/modules/shared.module';

import { AddModifyAdminUserComponent } from '../../components/admin/addAdminUser/addmodifyadminuser.component';

import { AdminUserListComponent } from '../../components/admin/addAdminUser/adminuserlist.component';





@NgModule({
  declarations: [
  
    AdminUserListComponent,
    AddModifyAdminUserComponent,
 
    
  ],
  imports: [
   
    CommonModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    AdminModuleRoutingModule,

  ],
  exports: []
})
export class AdminModuleModule { }
