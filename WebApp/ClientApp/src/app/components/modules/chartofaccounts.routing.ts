import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfileComponent } from '../../components/profile/profile.component';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { ChartOfAccountComponent } from '../../components/chartOfAccount/chartOfAccount.component'
import { ModifyChartOfAccountComponent } from '../../components/chartOfAccount/modifyChartOfAccount.component';
const routes: Routes = [
  {
    path: '', canActivate: [AuthGuard], children:[
      { path: '', component: ChartOfAccountComponent },
      { path: 'modify', component: ModifyChartOfAccountComponent },
      { path: 'add', component: ModifyChartOfAccountComponent },
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChartOfAccountsRoutingModule { }
