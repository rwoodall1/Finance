import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProfileComponent } from '../../components/profile/profile.component';
import { AuthGuard } from '../../shared/guards/auth-guard.service';
const routes: Routes = [
  {
    path: '', canActivate: [AuthGuard], children:[
      { path: '', component: ProfileComponent },
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
