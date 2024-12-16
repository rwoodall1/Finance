import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReconcileComponent } from '../../components/reconcile/reconcile.component'
import { AuthGuard } from '../../shared/guards/auth-guard.service';

const routes: Routes = [
  {
    path: '', canActivate: [AuthGuard], children:[
      { path: '', component: ReconcileComponent },
      
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReconcileRoutingModule { }
