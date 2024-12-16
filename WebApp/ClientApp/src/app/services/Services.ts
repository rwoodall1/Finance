import { Injectable, ElementRef, EventEmitter,Output } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of, throwError as observableThrowError, Subscription } from 'rxjs';
import { delay } from 'rxjs/operators';
import { map, catchError, last } from 'rxjs/operators';
import { Constants,Global  } from '../shared/global';
import { environment } from '../../environments/environment';
import { ApiProcessingResult, ApiProcessingError } from '../bindingmodels/coreBindingModel';
import { LoggedInUser } from '../bindingmodels/userBindingModel';
import { BankFeedModel } from '../bindingmodels/bankFeedModels';
import { CurrencyPipe, NgLocaleLocalization } from '@angular/common';
import { TransActionCrudModel } from '../bindingmodels/transActionBindingModels';
import {  EmailModel, EnvironmentInfo,} from '../bindingmodels/miscBindingModels';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NotificationService } from '../services/notification.service'
import { JwtHelperService } from '@auth0/angular-jwt';
import * as moment from 'moment'; 
import { saveAs as importedSaveAs } from "file-saver";
import * as XLSX from 'xlsx';
import { ExcelJson } from '../interfaces/excel-json.interface';
import { CountDownDialogComponent } from '../components/Dialogs/countDownDialog/countDownDialog.component';

//import { YesNo, UTCToLocalFilter } from '../shared/filters';


const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
};

@Injectable()
export class BaseService {
    rootUrl: string = environment.apiURL;

    constructor(private _http: HttpClient, private _constants: Constants) {
        //_http is sent from inherited class
    }

    //base calls
  get(url: string): Observable<any> {
   return this._http.get(url, httpOptions)
      .pipe(catchError(this.handleError));
  
  }
  getBlob(url: string): Observable<any> {

    return this._http.get(url, {
      reportProgress: true,
      observe: 'events',
      responseType: 'blob'
    })
      .pipe(catchError(this.handleError));

  }
  post(url: string, model: any): Observable<any> {
     
        let requestData = JSON.stringify(model);
    return this._http.post(url, requestData, httpOptions)
          .pipe(catchError(this.handleError));
    }
  postFormData(url: string, formData: FormData): Observable<any> {
    return this._http.post(url, formData)
      .pipe(catchError(this.handleError));
  }
   

  private sleep(ms: number) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++) {
            if ((new Date().getTime() - start) > ms) {
                break;
            }
        }
  }
  private handleError(error: HttpErrorResponse) {
    console.error(error);
    var err = new ApiProcessingError(error.statusText, error.statusText, error.status.toString());
    //err.errorCode = error.status.toString();
    //err.errorMessage = error.statusText;
    var result = new ApiProcessingResult<any>();
    result.isError = true;
    result.errors = [];
    result.errors.push(err);
    return observableThrowError(result);
    //return observableThrowError(error || 'Server error');
  }

  
}


@Injectable()
export class UtilService extends BaseService {
    //baseUrl: string = 'utility/';

    //constructor(private vhttp: HttpClient, private constants: Constants, private global: Global, public router: Router) {
    //    super(vhttp, constants);
    //}

    //goToTop() {
    //    window.scrollTo(0, 0);
    //}
    //getServerVars() {
    //    return this.get(this.baseUrl + 'getServerVars');
    //}

    //searchList(nameKey, myArray, type) {
    //    for (var i = 0; i < myArray.length; i++) {
    //        if (myArray[i][type] === nameKey) {
    //            return i;
    //        }
    //    }
    //}

    //splitString(paramArray, paramDelimiter?) {
    //    var delimiter = ",";
    //    if (typeof paramDelimiter !== 'undefined' && paramDelimiter !== null) {
    //        delimiter = paramDelimiter;
    //    }
    //    var a = paramArray.split(delimiter)
    //    if (a[0] == '' && a.length == 1) return [];
    //    return a;
    //}

    //goBack() {
    //    window.history.back();
    //}

    //getGridHeight(gridRows, hasFooter, rowHeight) {
    //    var myHeight = 550;
    //    var myRowHeight = rowHeight || 30;
    //    if (gridRows > 20) {
    //        myHeight = 20 + (gridRows * myRowHeight) + 10; //Header height + row heights (times number of rows) + viewport padding (don't know why)
    //        if (hasFooter) { myHeight = myHeight + 25; }
    //    }
    //    return myHeight;
    //}
}
@Injectable()
export class BankFeedService extends BaseService {
  constructor(private vhttp: HttpClient, private constants: Constants) {
    super(vhttp, constants);
    this.rootUrl = environment.apiURL;
  }
  baseUrl: string = environment.apiURL + 'feed/';
  readBankFeed(formData: FormData) {
  
    return this.postFormData(this.baseUrl + 'readBankFeed', formData);
  }
  getImportedData(accountId:string) {

    return this.get(this.baseUrl + 'getImportedData?bankId=' + accountId)

  }
  importData(data:Array<BankFeedModel>) {
    return this.post(this.baseUrl + 'importData', data);

  }
}
@Injectable()
export class ValidationService extends BaseService {
  constructor(private vhttp: HttpClient, private constants: Constants) {
    super(vhttp, constants);
    this.rootUrl = environment.apiURL;
  }
  validateTransaction(parentData: TransActionCrudModel, childData: Array<TransActionCrudModel>): ApiProcessingResult<any> {
    let processingResult = new ApiProcessingResult();
    if (!parentData.accountId || parentData.accountId==0) {
      processingResult.isError = true;
      processingResult.errors.push(new ApiProcessingError('Parent Account Id is missing', 'Parent Account Id is missing',''))
      return processingResult;
    }
    let dataList = new Array<TransActionCrudModel>();
   

    childData.forEach(record => {
      switch (0) {

        case record.accountId:
          processingResult.isError = true;
          processingResult.errors.push(new ApiProcessingError('Missing child account, be sure you have assigned an account for each line of the transaction', 'Missing child account, be sure you have assigned an account for each line of the transaction', ''))
          break;
       
      }

    })
    if (processingResult.isError) {
      return processingResult;

    }
    //assign child account to parent
    if (childData.length > 1) {
      parentData.childAccountId = 1
    } else {
      parentData.childAccountId=childData[0].accountId
    }
    dataList.push(parentData);

    //make sure data is correct between parent and child
    childData.forEach(rec => {
      rec.childAccountId = parentData.accountId;
      rec.clr = parentData.clr;
      rec.refNumber = parentData.refNumber;
      rec.payeeId = parentData.payeeId;
      rec.reconciled = parentData.reconciled;
      rec.transDate = parentData.transDate;
      rec.transType = parentData.transType;
      dataList.push(rec);
    })

    //check balance
    let _parentBalance = 0;
    let _childBalance = 0;
    //SET PARENT VALUE
    if (parentData.credit > 0 && parentData.debit) {
      processingResult.isError = true;
      processingResult.errors.push(new ApiProcessingError('Parent balance error', 'Parent balance error', ''))

    } else if (parentData.credit > 0) {
      _parentBalance = +parentData.credit;
    } else if (parentData.debit > 0) {
      _parentBalance = +parentData.debit

    }
  //set child value
    childData.forEach(item => {

      _childBalance += ((item.credit == null ? 0 : +item.credit) + (item.debit == null ? 0 : +item.debit))

    })
    
    if (_childBalance != _parentBalance) {

      processingResult.isError = true;
      processingResult.errors.push(new ApiProcessingError('Transaction is not in balance', 'Transaction is not in balance', ''))
      return processingResult;
    }

    processingResult.data = dataList;

return processingResult;
  }
  

}
@Injectable()
export class AuthService {

  tokenSubscription = new Subscription()
  tokenNoticeSubscription = new Subscription();
  timeout;
  
  constructor(public jwtHelper: JwtHelperService,public dialog: MatDialog, private Global: GlobalService, private Notification: NotificationService, private router: Router, private http: HttpClient) {


  }

  authenticateUser(credentials: Object) {
    const loginUrl = environment.loginURL
    let a = loginUrl;
    return this.http.post<ApiProcessingResult<LoggedInUser>>(loginUrl, credentials, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    }).pipe(
      map((response) => {
        let result = response.apiProcessingResult;

        if (result.isError) {
          return result;
        }
        const token = result.data.token;
        localStorage.setItem("jwt", token);
        this.Global.setLoggedInUser(result.data)
        this.expirationCounterMsg();
        this.expirationCounter();

        return result;
      })
    )
  }

  expirationCounterMsg() {
    let token = localStorage.getItem("jwt")

    this.timeout = this.jwtHelper.getTokenExpirationDate(token).valueOf() - new Date().valueOf();
    let vdelay: number;
    if (this.timeout > 30000) {
      vdelay = this.timeout - 30000;
    } else {
      vdelay = 0;

    }


    this.tokenNoticeSubscription.unsubscribe();
    this.tokenNoticeSubscription = of(null).pipe(delay(vdelay)).subscribe((expired) => {


      const dialogConfig = new MatDialogConfig();
      dialogConfig.width = "300px";
      dialogConfig.height = "40px";
      dialogConfig.hasBackdrop = false;
      dialogConfig.panelClass = 'countDown';
      dialogConfig.position = {
        'top': '80px',
        left: '0'
      };

      dialogConfig.disableClose = true;

      const dialogRef = this.dialog.open(CountDownDialogComponent, dialogConfig)

    });

  }

  expirationCounter() {
    let token = localStorage.getItem("jwt")
    this.timeout = this.jwtHelper.getTokenExpirationDate(token).valueOf() - new Date().valueOf();
    this.tokenSubscription.unsubscribe();
    this.tokenSubscription = of(null).pipe(delay(this.timeout)).subscribe((expired) => {
      console.log('EXPIRED!!');

      this.logout();

    });
  }
  logout() {

    this.tokenSubscription.unsubscribe();
    localStorage.clear();
    this.Notification.displayWarning("You have been logged out, please login again.")
    this.router.navigate(['/login']);
  }




}



@Injectable()
export class TransActionService extends BaseService {
  baseUrl: string = environment.apiURL + 'transaction/';
  constructor(private vhttp: HttpClient, private constants: Constants) {
    super(vhttp, constants);
    this.rootUrl = environment.apiURL;
  }
  setReconciled(data) {
    return this.post(this.baseUrl + 'setReconciled', data);

  }
  insertUpdateReconciliation(data) {

    return this.post(this.baseUrl + 'insertUpdateReconciliation', data);
  }
  setCleared(data: Array<TransActionCrudModel>) {
    return this.post(this.baseUrl + 'setCleared', data);
  }
  getChildTransActions(transactionId,accountId) {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getChildTransActions?transactionId='+transactionId+'&accountId=' + accountId);

  }
  getTransActions(accountId,reconciled) {
    this.rootUrl = environment.apiURL;
    if (!reconciled) {
      return this.get(this.baseUrl + 'getTransActions?accountId=' + accountId);
    } else {
      return this.get(this.baseUrl + 'getUnReconciledTransActions?accountId=' + accountId);
    }

  }

  deleteTransAction(transactionId) {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'deleteTransAction?transId=' + transactionId);
 
  }
  upDateTransAction(model:any) {
    this.rootUrl = environment.apiURL;
    return this.post(this.baseUrl + 'update',model);

  }
  newTransaction(model: any) {
    return this.post(this.baseUrl + 'newTransaction', model);

  }
  getLastStatement(accountId:number) {
    return this.get(this.baseUrl + 'getLastStatementBalance?accountId=' + accountId);
  }
}
@Injectable()
export class UserService extends BaseService {
  baseUrl: string = environment.apiURL+ 'user/';
    constructor(private vhttp: HttpClient, private constants: Constants) {
        super(vhttp, constants);
        this.rootUrl = environment.apiURL;
  }
  addAdminUser(adminuser) {
    this.rootUrl = environment.apiURL;
    return this.post(this.baseUrl + 'addAdminUser', adminuser);

  }
  updateAdminUser(adminuser) {
    this.rootUrl = environment.apiURL;
    return this.post(this.baseUrl + 'updateAdminUser',adminuser);

  }
  getAdminUsers() {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getAdminUsers');

  }
  register(data) {
        this.rootUrl = environment.apiURL;
        return this.post(this.baseUrl + 'register', data);
  }

  getUserProfile(userId:string) {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getUserProfile?userId=' + userId);
  }
  updateUserProfile(user) {
    this.rootUrl = environment.apiURL;
    return this.post(this.baseUrl + 'updateUserProfile', user);
  }

  deleteUser(userId:string) {
    return this.get(this.baseUrl + 'deleteUser?userid='+userId);

  }
}
@Injectable()
export class GlobalService extends BaseService {
  baseUrl: string = environment.apiURL + 'user/';
  constructor(private vhttp: HttpClient, private constants: Constants, public router: Router, public global: Global,) {
    super(vhttp, constants);
    this.rootUrl = environment.apiURL;
  }
  @Output()
  impersonateUserChange = new EventEmitter<void>();
  getImpersonatedChangedEmmitter() {
    return this.impersonateUserChange;
  }

 

  getLookUpAccounts() {
    const url = this.rootUrl +'accounts/'
    return this.get(url + 'getSysAccounts');
  }



  EnvironmentInfo= new EnvironmentInfo();

  
//  hasAdminAccess() {
//    //var isSalesUser = this.getLoggedInUser().roles.indexOf("Sales") > -1;
//    //return (!isSalesUser || (isSalesUser && (this.isCorpUser() || this.isDistUser())));
//    return false;
//  }
  newGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      let r = Math.random() * 16 | 0
       const v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  

  getLoggedInUser(): LoggedInUser {
    let loggedInUser: LoggedInUser;
    if ( !localStorage.getItem("loggedInUser")) {
      return null;
    } else if (localStorage.getItem("loggedInUser")) {
      loggedInUser = JSON.parse(localStorage.getItem("loggedInUser"));
    }
    return loggedInUser;   
  }
  setLoggedInUser(user: LoggedInUser): void {
   
    if (user !== null) {
     
      localStorage.setItem("loggedInUser", JSON.stringify(user));
    }
  }
  setEnvironmentInfo(data: EnvironmentInfo) {
    this.EnvironmentInfo.environment = data.environment;
    this.EnvironmentInfo.isDeveloperMachine = data.isDeveloperMachine;

  }
  getEnvironmentInfo():EnvironmentInfo{
     return this.EnvironmentInfo

   }

  
}
@Injectable()
export class AccountService extends BaseService {
  baseUrl: string = environment.apiURL + 'accounts/';
  constructor(private vhttp: HttpClient, private constants: Constants) {
    super(vhttp, constants);
    this.rootUrl = environment.apiURL;
  }
  sysAccounts() {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getSysAccounts');
  }
  saveAccount(account) {
    this.rootUrl = environment.apiURL;
    return this.post(this.baseUrl + 'saveAccount',account);

  }
  getSubAccounts(type) {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getSubAccounts?type='+type);

  }
  getAllAccounts() {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getChartOfAccounts');

  }
  getAccount(accountId) {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getAccount?id='+accountId);

  }
  getAccountTypes() {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getAccountTypes');

  }
  getBankAccounts() {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getBankAccounts');

  }
}
@Injectable()
export class NameService extends BaseService {
  baseUrl: string = environment.apiURL + 'names/';
  constructor(private vhttp: HttpClient, private constants: Constants) {
    super(vhttp, constants);
    this.rootUrl = environment.apiURL;
  }
  getNames() {
    this.rootUrl = environment.apiURL;
    return this.get(this.baseUrl + 'getNames');

  }

}









@Injectable()
export class NodeService {
  getTreeNodesData() {
    return [
      {
        key: '0',
        label: 'Documents',
        data: 'Documents Folder',
        icon: 'pi pi-fw pi-inbox',
        children: [
          {
            key: '0-0',
            label: 'Work',
            data: 'Work Folder',
            icon: 'pi pi-fw pi-cog',
            children: [
              { key: '0-0-0', label: 'Expenses.doc', icon: 'pi pi-fw pi-file', data: 'Expenses Document' },
              { key: '0-0-1', label: 'Resume.doc', icon: 'pi pi-fw pi-file', data: 'Resume Document' }
            ]
          },
          {
            key: '0-1',
            label: 'Home',
            data: 'Home Folder',
            icon: 'pi pi-fw pi-home',
            children: [{ key: '0-1-0', label: 'Invoices.txt', icon: 'pi pi-fw pi-file', data: 'Invoices for this month' }]
          }
        ]
      },
      {
        key: '1',
        label: 'Events',
        data: 'Events Folder',
        icon: 'pi pi-fw pi-calendar',
        children: [
          { key: '1-0', label: 'Meeting', icon: 'pi pi-fw pi-calendar-plus', data: 'Meeting' },
          { key: '1-1', label: 'Product Launch', icon: 'pi pi-fw pi-calendar-plus', data: 'Product Launch' },
          { key: '1-2', label: 'Report Review', icon: 'pi pi-fw pi-calendar-plus', data: 'Report Review' }
        ]
      },
      {
        key: '2',
        label: 'Movies',
        data: 'Movies Folder',
        icon: 'pi pi-fw pi-star-fill',
        children: [
          {
            key: '2-0',
            icon: 'pi pi-fw pi-star-fill',
            label: 'Al Pacino',
            data: 'Pacino Movies',
            children: [
              { key: '2-0-0', label: 'Scarface', icon: 'pi pi-fw pi-video', data: 'Scarface Movie' },
              { key: '2-0-1', label: 'Serpico', icon: 'pi pi-fw pi-video', data: 'Serpico Movie' }
            ]
          },
          {
            key: '2-1',
            label: 'Robert De Niro',
            icon: 'pi pi-fw pi-star-fill',
            data: 'De Niro Movies',
            children: [
              { key: '2-1-0', label: 'Goodfellas', icon: 'pi pi-fw pi-video', data: 'Goodfellas Movie' },
              { key: '2-1-1', label: 'Untouchables', icon: 'pi pi-fw pi-video', data: 'Untouchables Movie' }
            ]
          }
        ]
      }
    ];
  }

  getTreeTableNodesData() {
    return [
      {
        key: '0',
        data: {
          name: 'Applications',
          size: '100kb',
          type: 'Folder'
        },
        children: [
          {
            key: '0-0',
            data: {
              name: 'React',
              size: '25kb',
              type: 'Folder'
            },
            children: [
              {
                key: '0-0-0',
                data: {
                  name: 'react.app',
                  size: '10kb',
                  type: 'Application'
                }
              },
              {
                key: '0-0-1',
                data: {
                  name: 'native.app',
                  size: '10kb',
                  type: 'Application'
                }
              },
              {
                key: '0-0-2',
                data: {
                  name: 'mobile.app',
                  size: '5kb',
                  type: 'Application'
                }
              }
            ]
          },
          {
            key: '0-1',
            data: {
              name: 'editor.app',
              size: '25kb',
              type: 'Application'
            }
          },
          {
            key: '0-2',
            data: {
              name: 'settings.app',
              size: '50kb',
              type: 'Application'
            }
          }
        ]
      },
      {
        key: '1',
        data: {
          name: 'Cloud',
          size: '20kb',
          type: 'Folder'
        },
        children: [
          {
            key: '1-0',
            data: {
              name: 'backup-1.zip',
              size: '10kb',
              type: 'Zip'
            }
          },
          {
            key: '1-1',
            data: {
              name: 'backup-2.zip',
              size: '10kb',
              type: 'Zip'
            }
          }
        ]
      },
      {
        key: '2',
        data: {
          name: 'Desktop',
          size: '150kb',
          type: 'Folder'
        },
        children: [
          {
            key: '2-0',
            data: {
              name: 'note-meeting.txt',
              size: '50kb',
              type: 'Text'
            }
          },
          {
            key: '2-1',
            data: {
              name: 'note-todo.txt',
              size: '100kb',
              type: 'Text'
            }
          }
        ]
      },
      {
        key: '3',
        data: {
          name: 'Documents',
          size: '75kb',
          type: 'Folder'
        },
        children: [
          {
            key: '3-0',
            data: {
              name: 'Work',
              size: '55kb',
              type: 'Folder'
            },
            children: [
              {
                key: '3-0-0',
                data: {
                  name: 'Expenses.doc',
                  size: '30kb',
                  type: 'Document'
                }
              },
              {
                key: '3-0-1',
                data: {
                  name: 'Resume.doc',
                  size: '25kb',
                  type: 'Resume'
                }
              }
            ]
          },
          {
            key: '3-1',
            data: {
              name: 'Home',
              size: '20kb',
              type: 'Folder'
            },
            children: [
              {
                key: '3-1-0',
                data: {
                  name: 'Invoices',
                  size: '20kb',
                  type: 'Text'
                }
              }
            ]
          }
        ]
      },
      {
        key: '4',
        data: {
          name: 'Downloads',
          size: '25kb',
          type: 'Folder'
        },
        children: [
          {
            key: '4-0',
            data: {
              name: 'Spanish',
              size: '10kb',
              type: 'Folder'
            },
            children: [
              {
                key: '4-0-0',
                data: {
                  name: 'tutorial-a1.txt',
                  size: '5kb',
                  type: 'Text'
                }
              },
              {
                key: '4-0-1',
                data: {
                  name: 'tutorial-a2.txt',
                  size: '5kb',
                  type: 'Text'
                }
              }
            ]
          },
          {
            key: '4-1',
            data: {
              name: 'Travel',
              size: '15kb',
              type: 'Text'
            },
            children: [
              {
                key: '4-1-0',
                data: {
                  name: 'Hotel.pdf',
                  size: '10kb',
                  type: 'PDF'
                }
              },
              {
                key: '4-1-1',
                data: {
                  name: 'Flight.pdf',
                  size: '5kb',
                  type: 'PDF'
                }
              }
            ]
          }
        ]
      },
      {
        key: '5',
        data: {
          name: 'Main',
          size: '50kb',
          type: 'Folder'
        },
        children: [
          {
            key: '5-0',
            data: {
              name: 'bin',
              size: '50kb',
              type: 'Link'
            }
          },
          {
            key: '5-1',
            data: {
              name: 'etc',
              size: '100kb',
              type: 'Link'
            }
          },
          {
            key: '5-2',
            data: {
              name: 'var',
              size: '100kb',
              type: 'Link'
            }
          }
        ]
      },
      {
        key: '6',
        data: {
          name: 'Other',
          size: '5kb',
          type: 'Folder'
        },
        children: [
          {
            key: '6-0',
            data: {
              name: 'todo.txt',
              size: '3kb',
              type: 'Text'
            }
          },
          {
            key: '6-1',
            data: {
              name: 'logo.png',
              size: '2kb',
              type: 'Picture'
            }
          }
        ]
      },
      {
        key: '7',
        data: {
          name: 'Pictures',
          size: '150kb',
          type: 'Folder'
        },
        children: [
          {
            key: '7-0',
            data: {
              name: 'barcelona.jpg',
              size: '90kb',
              type: 'Picture'
            }
          },
          {
            key: '7-1',
            data: {
              name: 'primeng.png',
              size: '30kb',
              type: 'Picture'
            }
          },
          {
            key: '7-2',
            data: {
              name: 'prime.jpg',
              size: '30kb',
              type: 'Picture'
            }
          }
        ]
      },
      {
        key: '8',
        data: {
          name: 'Videos',
          size: '1500kb',
          type: 'Folder'
        },
        children: [
          {
            key: '8-0',
            data: {
              name: 'primefaces.mkv',
              size: '1000kb',
              type: 'Video'
            }
          },
          {
            key: '8-1',
            data: {
              name: 'intro.avi',
              size: '500kb',
              type: 'Video'
            }
          }
        ]
      }
    ];
  }

  getLazyNodesData() {
    return [
      {
        "label": "Lazy Node 0",
        "data": "Node 0",
        "expandedIcon": "pi pi-folder-open",
        "collapsedIcon": "pi pi-folder",
        "leaf": false
      },
      {
        "label": "Lazy Node 1",
        "data": "Node 1",
        "expandedIcon": "pi pi-folder-open",
        "collapsedIcon": "pi pi-folder",
        "leaf": false
      },
      {
        "label": "Lazy Node 1",
        "data": "Node 2",
        "expandedIcon": "pi pi-folder-open",
        "collapsedIcon": "pi pi-folder",
        "leaf": false
      }
    ]
  }

  getFileSystemNodesData() {
    return [
      {
        "data": {
          "name": "Applications",
          "size": "200mb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "Angular",
              "size": "25mb",
              "type": "Folder"
            },
            "children": [
              {
                "data": {
                  "name": "angular.app",
                  "size": "10mb",
                  "type": "Application"
                }
              },
              {
                "data": {
                  "name": "cli.app",
                  "size": "10mb",
                  "type": "Application"
                }
              },
              {
                "data": {
                  "name": "mobile.app",
                  "size": "5mb",
                  "type": "Application"
                }
              }
            ]
          },
          {
            "data": {
              "name": "editor.app",
              "size": "25mb",
              "type": "Application"
            }
          },
          {
            "data": {
              "name": "settings.app",
              "size": "50mb",
              "type": "Application"
            }
          }
        ]
      },
      {
        "data": {
          "name": "Cloud",
          "size": "20mb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "backup-1.zip",
              "size": "10mb",
              "type": "Zip"
            }
          },
          {
            "data": {
              "name": "backup-2.zip",
              "size": "10mb",
              "type": "Zip"
            }
          }
        ]
      },
      {
        "data": {
          "name": "Desktop",
          "size": "150kb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "note-meeting.txt",
              "size": "50kb",
              "type": "Text"
            }
          },
          {
            "data": {
              "name": "note-todo.txt",
              "size": "100kb",
              "type": "Text"
            }
          }
        ]
      },
      {
        "data": {
          "name": "Documents",
          "size": "75kb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "Work",
              "size": "55kb",
              "type": "Folder"
            },
            "children": [
              {
                "data": {
                  "name": "Expenses.doc",
                  "size": "30kb",
                  "type": "Document"
                }
              },
              {
                "data": {
                  "name": "Resume.doc",
                  "size": "25kb",
                  "type": "Resume"
                }
              }
            ]
          },
          {
            "data": {
              "name": "Home",
              "size": "20kb",
              "type": "Folder"
            },
            "children": [
              {
                "data": {
                  "name": "Invoices",
                  "size": "20kb",
                  "type": "Text"
                }
              }
            ]
          }
        ]
      },
      {
        "data": {
          "name": "Downloads",
          "size": "25mb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "Spanish",
              "size": "10mb",
              "type": "Folder"
            },
            "children": [
              {
                "data": {
                  "name": "tutorial-a1.txt",
                  "size": "5mb",
                  "type": "Text"
                }
              },
              {
                "data": {
                  "name": "tutorial-a2.txt",
                  "size": "5mb",
                  "type": "Text"
                }
              }
            ]
          },
          {
            "data": {
              "name": "Travel",
              "size": "15mb",
              "type": "Text"
            },
            "children": [
              {
                "data": {
                  "name": "Hotel.pdf",
                  "size": "10mb",
                  "type": "PDF"
                }
              },
              {
                "data": {
                  "name": "Flight.pdf",
                  "size": "5mb",
                  "type": "PDF"
                }
              }
            ]
          }
        ]
      },
      {
        "data": {
          "name": "Main",
          "size": "50mb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "bin",
              "size": "50kb",
              "type": "Link"
            }
          },
          {
            "data": {
              "name": "etc",
              "size": "100kb",
              "type": "Link"
            }
          },
          {
            "data": {
              "name": "var",
              "size": "100kb",
              "type": "Link"
            }
          }
        ]
      },
      {
        "data": {
          "name": "Other",
          "size": "5mb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "todo.txt",
              "size": "3mb",
              "type": "Text"
            }
          },
          {
            "data": {
              "name": "logo.png",
              "size": "2mb",
              "type": "Picture"
            }
          }
        ]
      },
      {
        "data": {
          "name": "Pictures",
          "size": "150kb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "barcelona.jpg",
              "size": "90kb",
              "type": "Picture"
            }
          },
          {
            "data": {
              "name": "primeng.png",
              "size": "30kb",
              "type": "Picture"
            }
          },
          {
            "data": {
              "name": "prime.jpg",
              "size": "30kb",
              "type": "Picture"
            }
          }
        ]
      },
      {
        "data": {
          "name": "Videos",
          "size": "1500mb",
          "type": "Folder"
        },
        "children": [
          {
            "data": {
              "name": "primefaces.mkv",
              "size": "1000mb",
              "type": "Video"
            }
          },
          {
            "data": {
              "name": "intro.avi",
              "size": "500mb",
              "type": "Video"
            }
          }
        ]
      }
    ]
  }

  getTreeTableNodes() {
    return Promise.resolve(this.getTreeTableNodesData());
  }

  getTreeNodes() {
    return Promise.resolve(this.getTreeNodesData());
  }

  getFiles() {
    return Promise.resolve(this.getTreeNodesData());
  }

  getLazyFiles() {
    return Promise.resolve(this.getLazyNodesData());
  }

  getFilesystem() {
    return Promise.resolve(this.getFileSystemNodesData());
  }

};

const EXCEL_EXTENSION = '.xlsx';
const CSV_EXTENSION = '.csv';
const CSV_TYPE = 'text/plain;charset=utf-8';
@Injectable()
export class ExportService {
  constructor(  private constants: Constants, public global: Global) {
    //https://dev.to/idrisrampurawala/exporting-getLazyNodes-to-excel-and-csv-in-angular-3643
  }

  /**
    * Creates excel from the table element reference.
    *
    * @param element DOM table element reference.
    * @param fileName filename to save as.
    */
  
  public exportTableElmToExcel(element: ElementRef, fileName: string): void {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element.nativeElement);
    // generate workbook and add the worksheet
    const workbook: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, ws, 'Sheet1');
    // save to file
    XLSX.writeFile(workbook, `${fileName}${EXCEL_EXTENSION}`);

  }

  /**
   * Creates XLSX option from the Json getLazyNodes. Use this to customise the sheet by adding arbitrary rows and columns.
   *
   * @param json Json data to create xlsx.
   * @param fileName filename to save as.
   */
  public exportJsonToExcel(json: ExcelJson[], fileName: string): void {
    // inserting first blank row
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
      json[0].data,
      this.getOptions(json[0])
    );

    for (let i = 1, length = json.length; i < length; i++) {
      // adding a dummy row for separation
      XLSX.utils.sheet_add_json(
        worksheet,
        [{}],
        this.getOptions(
          {
            data: [],
            skipHeader: true
          }, -1)
      );
      XLSX.utils.sheet_add_json(
        worksheet,
        json[i].data,
        this.getOptions(json[i], -1)
      );
    }
    const workbook: XLSX.WorkBook = { Sheets: { Sheet1: worksheet }, SheetNames: ['Sheet1'] };
    // save to file
    XLSX.writeFile(workbook, `${fileName}${EXCEL_EXTENSION}`);
  }

  /**
   * Creates XLSX option from the data.
   *
   * @param json Json data to create xlsx.
   * @param origin XLSX option origin.
   * @returns options XLSX options.
   */
  private getOptions(json: ExcelJson, origin?: number): any {
    // adding actual data
    const options = {
      skipHeader: true,
      origin: -1,
      header: []
    };
    options.skipHeader = json.skipHeader ? json.skipHeader : false;
    if (!options.skipHeader && json.header && json.header.length) {
      options.header = json.header;
    }
    if (origin) {
      options.origin = origin ? origin : -1;
    }
    return options;
  }

  /**
   * Saves the file on client's machine via FileSaver library.
   *
   * @param buffer The data that need to be saved.
   * @param fileName File name to save as.
   * @param fileType File type to save as.
   */
  private saveAsFile(buffer: any, fileName: string, fileType: string): void {
    const data: Blob = new Blob([buffer], { type: fileType });
    importedSaveAs(data, fileName);
  }

  /**
   * Creates an array of data to csv. It will automatically generate title row based on object keys.
   *
   * @param rows array of data to be converted to CSV.
   * @param fileName filename to save as.
   * @param columns array of object properties to convert to CSV. If skipped, then all object properties will be used for CSV.
   */
  public exportToCsv(rows: object[], fileName: string, columns?: string[]): any {
    if (!rows || !rows.length) {
      return;
    }
    const separator = ',';
    const keys = Object.keys(rows[0]).filter(k => {
      if (columns?.length) {
        return columns.includes(k);
      } else {
        return true;
      }
    });
    const csvContent =
      keys.join(separator) +
      '\n' +
      rows.map(row => {
        return keys.map(k => {
          let cell = row[k] === null || row[k] === undefined ? '' : row[k];

          cell = cell instanceof Date
            ? moment(cell.toLocaleString()).format('L').toString() //I put moment in here, put in cell.toLocaleString() to make original rw
            : cell.toString().replace(/"/g, '""');// will need to be formated before it reaches here if date.
          if (cell.search(/("|,|\n)/g) >= 0) {
            cell = `"${cell}"`;
          }

          return cell;
        }).join(separator);
      }).join('\n');
    this.saveAsFile(csvContent, `${fileName}${CSV_EXTENSION}`, CSV_TYPE);
  }
   
  
}






