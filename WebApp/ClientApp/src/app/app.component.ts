

import { Global } from './shared/global';
import { HeaderComponent } from "./common/header"
import { SubHeaderComponent } from "./common/subheader"
import { Router, ActivatedRoute, ActivatedRouteSnapshot, Params, NavigationEnd } from '@angular/router';
import { Component, OnInit, ViewChild, ContentChild, Input, OnChanges, ViewEncapsulation, TemplateRef, Directive, OnDestroy, EmbeddedViewRef, ViewContainerRef } from "@angular/core"
import { EmitterService } from './services/emitter.service';

@Component({
    selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.css'],
    standalone: false
})
export class AppComponent {
  title = 'app';


  constructor( public router: Router, private activatedRoute: ActivatedRoute, private _global: Global) {
 
    
  }
  ngOnInit(): void {

    if (window.location.hostname == 'pay.memorybook.com/') {
    this.router.navigate(["/schoolPay"]);
    }

  }
 
  get currentYear() {
    return new Date().getFullYear();
  }

  @ViewChild(HeaderComponent, { static: false }) headerComponent: HeaderComponent;

}

