"use strict";
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
exports.LoginComponent = void 0;
var http_1 = require("@angular/common/http");
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var platform_browser_1 = require("@angular/platform-browser");
var Services_1 = require("../../services/Services");
var LoginComponent = /** @class */ (function () {
    function LoginComponent(Global, router, http, domSanitizer) {
        this.Global = Global;
        this.router = router;
        this.http = http;
        this.domSanitizer = domSanitizer;
    }
    LoginComponent.prototype.ngOnInit = function () {
        this.styles = "<style>html, body { background-color:#e6eaed !important;}</style>";
    };
    LoginComponent.prototype.login = function (form) {
        var _this = this;
        var credentials = JSON.stringify(form.value);
        this.http.post("https://localhost:44365/auth/login", credentials, {
            headers: new http_1.HttpHeaders({
                "Content-Type": "application/json"
            })
        }).subscribe(function (response) {
            localStorage.setItem("jwt", response.data.token);
            _this.Global.setLoggedInUser(response.data);
            _this.invalidLogin = false;
            _this.router.navigate(["/"]);
        }, function (err) {
            _this.invalidLogin = true;
        });
    };
    LoginComponent.prototype.getStyles = function () {
        return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
    };
    LoginComponent = __decorate([
        core_1.Component({
            selector: 'login',
            templateUrl: './login.component.html'
        }),
        __metadata("design:paramtypes", [Services_1.GloabalService, router_1.Router, http_1.HttpClient, platform_browser_1.DomSanitizer])
    ], LoginComponent);
    return LoginComponent;
}());
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map