import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './common/header';
import { SubHeaderComponent } from './common/subheader';
import { FooterComponent } from './common/footer';
import { HomeComponent } from './components/home/home.component'
import { ChartOfAccountComponent } from './components/chartOfAccount/chartOfAccount.component'
import { AuthGuard } from './shared/guards/auth-guard.service';
import { LoginComponent } from './components/login/login.component';
import { ModuleWithProviders } from '@angular/core';
import { AdminGuard } from './shared/guards/admin-guard.service';
import { ModifyChartOfAccountComponent } from './components/chartOfAccount/modifyChartOfAccount.component'
import { RegisterComponent } from './components/register/register.component';
import { BankFeedComponent } from './components/bankFeed/bankFeed.component';
const appRoutes: Routes = [
 { //NON PROTECTED ROUTES
    path: '', children: [
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
 
    ] 
  },
  { //PROTECTED ROUTES
    path: '', canActivate: [AuthGuard], children: [
      { path: 'home', component: HomeComponent },
      { path: '', component: HeaderComponent, outlet: 'header' },
      { path: '', component: SubHeaderComponent, outlet: 'subheader' },
      { path: '', component: FooterComponent, outlet: 'footer' },
      { path: 'register', component: RegisterComponent },
      { path: 'reconcile', loadChildren: () => import('./components/modules/reconcile.module').then(m => m.ReconcileModule) },
      { path: 'chartOfAccounts', loadChildren: () => import('./components/modules/chartofaccounts.module').then(m => m.ChartOfAccountsModule) },
      { path: 'profile', loadChildren: () => import('./components/modules/profile.module').then(m => m.ProfileModule) },
      { path: 'import', loadChildren: () => import('./components/modules/bankFeed.module').then(m => m.BankFeedModule) },
      
    ]
 },
  { //PROTECTED ROUTES Admin
    path: '', canActivate: [AdminGuard], children: [
      { path: '', component: HeaderComponent, outlet: 'header' },
      { path: '', component: SubHeaderComponent, outlet: 'subheader' },
      { path: '', component: FooterComponent, outlet: 'footer' }, 
      { path: 'admin', loadChildren: () => import('./components/modules/admin.module').then(m => m.AdminModuleModule) },
    ]
  },

  { path: '**', redirectTo: '/404' } //404 Handler (THIS HAS TO BE THE LAST ROUTE ALWAYS)
]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})

export class AppRoutingModule { };

//export const routing: ModuleWithProviders<any> = RouterModule.forRoot(
//  appRoutes
//);





