import { Component, ViewEncapsulation, OnInit} from '@angular/core';
import { Global } from '../shared/global';
import { Router } from '@angular/router';
import {  DomSanitizer } from '@angular/platform-browser'
import { LoggedInUser } from '../bindingmodels/userBindingModel';
import { GlobalService,AuthService } from '../services/Services';
import { MatDialog,  MatDialogConfig,  MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { environment } from '../../environments/environment';
import { GetRegisterComponetComponent } from '../components/Dialogs/get-register-componet/get-register-componet.component'

@Component({
    selector: 'app-header',
    encapsulation: ViewEncapsulation.None,
    templateUrl: './header.html',
    styleUrls: ['./header.css'],
    standalone: false
})
  
export class HeaderComponent implements OnInit {
  styles: string;
  menuOpen: boolean;
  msg: string;
  user: LoggedInUser;
  isAdmin: boolean;
  isAdviser: boolean;
  isMobile: boolean;
  environment = environment;

  constructor(
    public authService: AuthService,
    public Global: GlobalService,
    public router: Router,
    private domSanitizer: DomSanitizer,
    public dialog: MatDialog, 
    public breakpointObserver: BreakpointObserver,
  ) {
    this.user = this.Global.getLoggedInUser();
    
  }

  ngOnInit(): void {
    this.styles = ``;
    this.user = this.Global.getLoggedInUser();
    this.isAdmin = this.user.role.rank <= 1;
    this.isAdviser = this.user.role.rank == 2;

    this.breakpointObserver
      .observe([Breakpoints.Small, Breakpoints.Handset])
      .subscribe(result => {
        this.isMobile = result.matches;
      });
    this.authService.expirationCounterMsg();
    this.authService.expirationCounter();

  }
  
 
  logout() {
    this.authService.logout();
  }

  getStyles() {
    return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
  }

 getBank(whatForm) {
    this.router.navigate(['/'])
    //what form is register or reconcile
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = "400px",
      dialogConfig.height = "500px",
      dialogConfig.closeOnNavigation = true,
      dialogConfig.data={type:whatForm }
  
    this.dialog.open(GetRegisterComponetComponent,dialogConfig)

  }
  importTransActions() {
    this.router.navigate(['/import'])

  }
}

