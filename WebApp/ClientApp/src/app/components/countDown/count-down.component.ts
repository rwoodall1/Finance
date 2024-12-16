import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import {map,takeWhile } from 'rxjs/operators';
import { timer } from 'rxjs';
import { JwtHelperService } from "@auth0/angular-jwt";

@Component({
    selector: 'count-down',
    templateUrl: './count-down.component.html',
    styleUrls: ['./count-down.component.css'],
    standalone: false
})
  //https://stackblitz.com/edit/rxjs-angular-countdown?file=src%2Fapp%2Fcount-down%2Fcount-down.component.ts,src%2Fapp%2Fapp.component.html
export class CountDownComponent {
  jwtHelper = new JwtHelperService();
  @Input() seconds = 0;
  @Output() endCountEvent = new EventEmitter<boolean>();
  thetime: number;
  token = localStorage.getItem("jwt")
  constructor() {
    
    this.run()
  }
 
  run() {
    let timeout = this.jwtHelper.getTokenExpirationDate(this.token).valueOf() - new Date().valueOf();
    if (timeout<0) {
      this.endCountEvent.emit(true);
      return;
    }else {
      this.seconds=timeout/1000
    }

 timer(0, 1000).pipe(
    map(n => (this.seconds - n) * 1000),
    takeWhile(n => n >= 0),
  ).subscribe(a => {
    try {
      
      let curtime = this.jwtHelper.getTokenExpirationDate(this.token).valueOf() - new Date().valueOf()
      if ((curtime / 1000) < 2) {
        this.seconds = 0;
        this.endCountEvent.emit(true);
      }
    } catch (e) {
      //errors if token has been cleared
      this.endCountEvent.emit(true);
    }
  
    this.thetime = a;
   
  });

  }
}
