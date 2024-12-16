import { AppRoutingModule } from './app.routing';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';


import { JwtModule, JWT_OPTIONS } from "@auth0/angular-jwt";
import { AuthGuard } from './shared/guards/auth-guard.service';
import { AdminGuard } from './shared/guards/admin-guard.service';
import { TokenInterceptor } from './token.interceptor';
import * as moment from 'moment';

import { ToastrModule } from 'ngx-toastr';
import { DecimalPipe } from '@angular/common';
import { SafeHtmlPipe, DecimalPlaces, SafeUrlPipe } from './directives/pipes';
import { environment } from '../environments/environment';
import { ModalModule } from './shared/modal';
import { ModalService } from './shared/modal/modal.service';
import { SharedModule } from './components/modules/shared.module';
import { EqualValidator, DigitOnlyDirective, ZeroValidator, AccountValidator } from './directives/validators'

//services
import { NotificationService } from './services/notification.service';

import { UserService, AccountService, NodeService, TransActionService, NameService, ValidationService,AuthService,BankFeedService } from './services/Services';
import { JwtHelperService } from '@auth0/angular-jwt';
import { GlobalService } from './services/Services';
import { ExportService } from './services/Services'

//custom
import { Global, Constants } from './shared/global';


//components
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component'
import { ForgotPasswordComponent } from './components/login/forgotPassword.component'
import { StyleComponent } from './components/style.component';
import { SubHeaderComponent } from './common/subheader';
import { FooterComponent } from './common/footer';
import { HeaderComponent } from './common/header';
import { CountDownComponent } from './components/countDown/count-down.component';
import { CountDownDialogComponent } from './components/Dialogs/countDownDialog/countDownDialog.component';
import { BeginReconComponent } from './components/Dialogs/beginRecon/beginRecon.component';
import { GetRegisterComponetComponent } from './components/Dialogs/get-register-componet/get-register-componet.component';
import { RegisterComponent } from './components/register/register.component';
import { AddModifytRowComponent } from './components/register/addModifyRow.component'

import { TableComponent } from './components/register/table.component';



@NgModule({
  declarations: [  
    AppComponent,   
    HomeComponent, 
    LoginComponent,
    ForgotPasswordComponent,
    HeaderComponent,
    FooterComponent,
    SubHeaderComponent,
    StyleComponent,
    SafeHtmlPipe,
    SafeUrlPipe,
    EqualValidator,
    AccountValidator,
    ZeroValidator,
    DigitOnlyDirective,
    AddModifytRowComponent,
    GetRegisterComponetComponent,
    BeginReconComponent,
    RegisterComponent,
    TableComponent,
    CountDownComponent,
    CountDownDialogComponent
 
  ],
  imports: [
    AppRoutingModule,
     BrowserModule,
    HttpClientModule,

    FormsModule,
    ReactiveFormsModule,
    ModalModule,
    BrowserAnimationsModule,
    
    SharedModule,
    ToastrModule.forRoot({
      autoDismiss: true,
      maxOpened: 0,
      newestOnTop: true,
      closeButton: true,
      positionClass: 'toast-bottom-full-width',
      preventDuplicates: false,
      tapToDismiss: true,
      timeOut:5000,
      extendedTimeOut:2000,
    }),
 
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  exports: [],
  providers: [BankFeedService,DecimalPipe, JwtHelperService, DecimalPlaces, NodeService, AuthService, ValidationService, NameService, ExportService, TransActionService, ModalService, AuthGuard, AdminGuard, NotificationService, AccountService, UserService,GlobalService,Global,Constants, { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
