import { Component, OnInit } from '@angular/core';
import { RouterStateSnapshot, Router } from '@angular/router';
import { DomSanitizer} from '@angular/platform-browser'
import { environment } from '../../environments/environment';

@Component({
    selector: "style",
    templateUrl: "./style.component.html",
    standalone: false
})

export class StyleComponent implements OnInit {
    styles: string;
    constructor(private router: Router, private domSanitizer: DomSanitizer) {

    }

    ngOnInit(): void {
      this.styles = `<style>html, body { background-color:#e6eaed !important;}</style>`
    }

    ngAfterViewInit(): void {

    }

    getStyles() {
        return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
    }
}

