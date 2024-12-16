"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.GloabalService = exports.UserService = exports.UtilService = exports.BaseService = void 0;
var core_1 = require("@angular/core");
var http_1 = require("@angular/common/http");
var router_1 = require("@angular/router");
var rxjs_1 = require("rxjs");
var operators_1 = require("rxjs/operators");
var global_1 = require("../shared/global");
var environment_1 = require("../../environments/environment");
var coreBindingModel_1 = require("../bindingmodels/coreBindingModel");
//import { Observable } from 'rxjs/Observable';
//import 'rxjs/add/operator/map';
//import 'rxjs/add/operator/do';
//import 'rxjs/add/operator/catch';
//import 'rxjs/add/observable/of';
//import { YesNo, UTCToLocalFilter } from '../shared/filters';
//import { Environment } from 'ag-grid';
//import { Http, Response, Headers, RequestOptions } from '@angular/http';
//import { Account } from '../bindingmodels/accountBindingModels';
//import { observeOn, } from 'rxjs/operators';
//import { log } from 'util';
//import { clearLine } from 'readline';
var httpOptions = {
    headers: new http_1.HttpHeaders({ 'Content-Type': 'application/json' })
};
var BaseService = /** @class */ (function () {
    function BaseService(_http, _constants) {
        this._http = _http;
        this._constants = _constants;
        this.rootUrl = environment_1.environment.apiURL;
        //_http is sent from inherited class
    }
    //base calls
    BaseService.prototype.get = function (url) {
        return this._http.get(url, httpOptions)
            .pipe(operators_1.catchError(this.handleError));
    };
    //get(url: string) {
    // return this._http.get(url,httpOptions).
    //    pipe(
    //      map((data: any[]) => {
    //        return data;
    //      }), catchError((err: HttpErrorResponse) => {
    //        return Observable.of(err);
    //      })
    //  )
    //}
    //get(url: string): Observable<any> {
    //    return this._http.get(this.rootUrl + url, httpOptions)
    //        .catch((err: HttpErrorResponse) => {
    //            return Observable.of(err);
    //  });
    //}
    //getWithoutHeaders(url: string): Observable<any> {
    //    return this._http.get(this.rootUrl + url)
    //        .catch((err: HttpErrorResponse) => {
    //            return Observable.of(err);
    //        });
    //}
    BaseService.prototype.post = function (url, model) {
        console.log(url);
        var requestData = JSON.stringify(model);
        return this._http.post(url, requestData, httpOptions)
            .pipe(operators_1.catchError(this.handleError));
    };
    //put(url: string, model: any): Observable<any> {
    //    let body = JSON.stringify(model);
    //    return this._http.put(this.rootUrl + url, body, httpOptions)
    //        .catch((err: HttpErrorResponse) => {
    //            return Observable.of(err);
    //        });
    //}
    //delete(url: string): Observable<any> {
    //    return this._http.delete(this.rootUrl + url, httpOptions)
    //        .catch((err: HttpErrorResponse) => {
    //            return Observable.of(err);
    //        });
    //}
    //private delay(ms: number) {
    //    return new Promise(resolve => setTimeout(resolve, ms));
    //}
    BaseService.prototype.sleep = function (ms) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++) {
            if ((new Date().getTime() - start) > ms) {
                break;
            }
        }
    };
    BaseService.prototype.handleError = function (error) {
        console.error(error);
        var err = new coreBindingModel_1.ApiProcessingError();
        err.errorCode = error.status.toString();
        err.errorMessage = error.statusText;
        var result = new coreBindingModel_1.ApiProcessingResult();
        result.isError = true;
        result.errors = [];
        result.errors.push(err);
        return rxjs_1.throwError(result);
        //return observableThrowError(error || 'Server error');
    };
    BaseService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient, global_1.Constants])
    ], BaseService);
    return BaseService;
}());
exports.BaseService = BaseService;
var UtilService = /** @class */ (function (_super) {
    __extends(UtilService, _super);
    function UtilService() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    UtilService = __decorate([
        core_1.Injectable()
    ], UtilService);
    return UtilService;
}(BaseService));
exports.UtilService = UtilService;
var UserService = /** @class */ (function (_super) {
    __extends(UserService, _super);
    function UserService(vhttp, constants, global) {
        var _this = _super.call(this, vhttp, constants) || this;
        _this.vhttp = vhttp;
        _this.constants = constants;
        _this.global = global;
        _this.baseUrl = environment_1.environment.apiURL + 'user/';
        _this.rootUrl = environment_1.environment.apiURL;
        return _this;
    }
    UserService.prototype.register = function (postData) {
        this.rootUrl = environment_1.environment.apiURL;
        return this.post(this.baseUrl + 'register', postData);
    };
    UserService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient, global_1.Constants, global_1.Global])
    ], UserService);
    return UserService;
}(BaseService));
exports.UserService = UserService;
var GloabalService = /** @class */ (function () {
    function GloabalService(vhttp, constants, global, router) {
        this.vhttp = vhttp;
        this.constants = constants;
        this.global = global;
        this.router = router;
    }
    GloabalService.prototype.hasAdminAccess = function () {
        //var isSalesUser = this.getLoggedInUser().roles.indexOf("Sales") > -1;
        //return (!isSalesUser || (isSalesUser && (this.isCorpUser() || this.isDistUser())));
        return false;
    };
    GloabalService.prototype.getLoggedInUser = function () {
        var loggedInUser;
        if (!localStorage.getItem("loggedInUser")) {
            return null;
        }
        else if (localStorage.getItem("loggedInUser")) {
            loggedInUser = JSON.parse(localStorage.getItem("loggedInUser"));
        }
        return loggedInUser;
    };
    GloabalService.prototype.setLoggedInUser = function (user) {
        if (user != null) {
            sessionStorage.setItem("loggedInUser", JSON.stringify(user));
        }
    };
    GloabalService.prototype.isLoggedIn = function () {
        var returnVar = false;
        if (sessionStorage.getItem('loggedInUser') || localStorage.getItem('loggedInUser')) {
            returnVar = true;
        }
        return returnVar;
    };
    //GloabalService.prototype.logout = function () {
    //    localStorage.clear();
    //    this.router.navigate(['/login']);
    //};
    GloabalService.prototype.getUrlParameterByName = function (name) {
        var url = window.location.href;
        name = name.replace(/[[]]/g, "\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"), results = regex.exec(url);
        if (!results)
            return null;
        if (!results[2])
            return '';
        return decodeURIComponent(results[2].replace("/+/g", " "));
    };
    GloabalService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient, global_1.Constants, global_1.Global, router_1.Router])
    ], GloabalService);
    return GloabalService;
}());
exports.GloabalService = GloabalService;
//# sourceMappingURL=Services.js.map
