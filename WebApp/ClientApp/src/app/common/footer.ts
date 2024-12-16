import { Component, ViewEncapsulation, OnInit }   from '@angular/core';

declare var require: any;

@Component({
    selector: 'app-footer',
    encapsulation: ViewEncapsulation.None,
    templateUrl: './footer.html',
    standalone: false
})
export class FooterComponent implements OnInit {

    constructor() {

    }
    ngOnInit(): void {
      
  }
  get currentYear() {
    return new Date().getFullYear();
  }
}
