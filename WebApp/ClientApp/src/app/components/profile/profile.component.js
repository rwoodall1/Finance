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
exports.RegisterComponent = void 0;
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var platform_browser_1 = require("@angular/platform-browser");
var Services_1 = require("../../services/Services");
var notification_service_1 = require("../../services/notification.service");
var Services_2 = require("../../services/Services");
var userBindingModel_1 = require("../../bindingmodels/userBindingModel");
require("rxjs/add/operator/mergeMap");
var common_1 = require("@angular/common");
var RegisterComponent = /** @class */ (function () {
    //client: ClientModel;
    function RegisterComponent(Global, Notification, _location, router, domSanitizer, userService) {
        this.Global = Global;
        this.Notification = Notification;
        this._location = _location;
        this.router = router;
        this.domSanitizer = domSanitizer;
        this.userService = userService;
    }
    RegisterComponent.prototype.ngOnInit = function () {
        this.user = new userBindingModel_1.NewUserModel();
        this.styles = "<style>html, body { background-color:#e6eaed !important;}</style>";
        this.hasLoaded = true;
    };
    RegisterComponent.prototype.getStyles = function () {
        return this.domSanitizer.bypassSecurityTrustHtml(this.styles);
    };
    RegisterComponent.prototype.save = function () {
        var _this = this;
        var lc = this;
        this.userService.register(this.user).subscribe(function (response) {
            var dataresponse = response.apiProcessingResult;
            if (dataresponse.isError) {
                _this.Notification.displayError(response.errors[0].errorMessage);
                return;
            }
            var token = dataresponse.data.token;
            if (token != null && token != '') {
                localStorage.setItem("jwt", token);
                _this.Global.setLoggedInUser(dataresponse.data);
                _this.router.navigate(["/"]);
            }
            else {
                _this.router.navigate(["/login"]);
            }
        });
    };
    RegisterComponent = __decorate([
        core_1.Component({
            selector: "register",
            templateUrl: "./register.component.html"
        }),
        __metadata("design:paramtypes", [Services_2.GloabalService, notification_service_1.NotificationService, common_1.Location, router_1.Router, platform_browser_1.DomSanitizer, Services_1.UserService])
    ], RegisterComponent);
    return RegisterComponent;
}());
exports.RegisterComponent = RegisterComponent;
//# sourceMappingURL=register.component.js.map