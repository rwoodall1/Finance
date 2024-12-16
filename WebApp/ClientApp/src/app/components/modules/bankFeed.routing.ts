import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminUserListComponent } from '../../components/admin/addAdminUser/adminuserlist.component';
import { BankFeedComponent } from '../../components/bankFeed/bankFeed.component';
import { AdminGuard } from '../../shared/guards/admin-guard.service';



//admin/route
const routes: Routes = [
  {
    path: '', canActivate: [AdminGuard], children: [
 
      { path: '', component: BankFeedComponent },
     
    ]
  }
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BankFeedRoutingModule { }
