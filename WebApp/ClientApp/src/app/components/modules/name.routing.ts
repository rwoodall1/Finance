import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfileComponent } from '../../components/profile/profile.component';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
import { NameComponent } from '../../components/name/name.component'
import { ModifyNameComponent } from '../../components/name/modifyName.component';
const routes: Routes = [
  {
    path: '', canActivate: [AuthGuard], children:[
      { path: '', component: NameComponent },
      { path: 'modify', component: ModifyNameComponent },
      { path: 'add', component: ModifyNameComponent },
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NameRoutingModule { }
