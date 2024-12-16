import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {  MatDialogModule } from '@angular/material/dialog';
import {  MatRadioModule, } from '@angular/material/radio';
import { MatInputModule, } from '@angular/material/input';
import {  MatCardModule, } from '@angular/material/card';
import { MatSelectModule, } from '@angular/material/select';
import {  MatListModule } from '@angular/material/list';
import {  MatCheckboxModule, } from '@angular/material/checkbox';
import { MatDatepickerModule, } from '@angular/material/datepicker';
import {  MatTabsModule, MatTabChangeEvent } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import {  MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import {  MatTooltipModule } from '@angular/material/tooltip';
import {  MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { Level, YesNoPipe } from '../../directives/pipes'
import { MatNativeDateModule } from '@angular/material/core';
import { CdkAccordionModule } from '@angular/cdk/accordion';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import {  MatPaginatorModule } from '@angular/material/paginator';
import { NgxSpinnerModule } from "ngx-spinner";
@NgModule({
  declarations: [
    Level,
    YesNoPipe,
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatInputModule,
    MatCardModule,
    MatSelectModule,
    MatTableModule,
     MatTooltipModule,
    MatButtonModule,
    MatListModule,
    MatMenuModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatTabsModule,
   
    MatRadioModule,
    MatNativeDateModule,
    MatBadgeModule,
    MatDividerModule,
    CdkAccordionModule,
    MatExpansionModule,
    MatIconModule,
    MatPaginatorModule,
    NgxSpinnerModule.forRoot({ type: 'ball-spin-clockwise' })
  ],
  exports: [CommonModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatInputModule,
    MatCardModule,
    MatSelectModule,
    MatTableModule,
    MatTooltipModule,
    MatButtonModule,
    MatMenuModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatTabsModule,
      MatRadioModule,
    MatNativeDateModule,
    MatBadgeModule,
    MatDividerModule,
    Level,
    YesNoPipe,
    CdkAccordionModule,
    MatExpansionModule,
    MatIconModule,
    MatPaginatorModule,
    NgxSpinnerModule
  ]
})
export class SharedModule { }


