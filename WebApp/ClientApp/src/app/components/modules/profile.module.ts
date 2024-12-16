import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../../components/modules/shared.module';
import { ProfileRoutingModule } from './profile.routing';
import { ModalModule } from '../../shared/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProfileComponent } from '../../components/profile/profile.component';
@NgModule({
  declarations: [ProfileComponent],
  imports: [
    CommonModule,
    ModalModule,
    FormsModule,
    ReactiveFormsModule,
    ProfileRoutingModule,
    SharedModule
  ]
})
export class ProfileModule { }
