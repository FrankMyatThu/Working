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
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var http_1 = require("@angular/http");
require("rxjs/add/operator/map");
var LoginUser_Binding_VM = (function () {
    function LoginUser_Binding_VM(UserName, Password) {
        this.UserName = UserName;
        this.Password = Password;
    }
    return LoginUser_Binding_VM;
}());
exports.LoginUser_Binding_VM = LoginUser_Binding_VM;
var LoginUser_Binding_VMs = [
    new LoginUser_Binding_VM('admin@admin.com', 'adm9'),
    new LoginUser_Binding_VM('user1@gmail.com', 'a23')
];
var AuthenticationService = (function () {
    function AuthenticationService(http, _router) {
        this.http = http;
        this._router = _router;
    }
    AuthenticationService.prototype.logout = function () {
        localStorage.removeItem("user");
        this._router.navigate(['login']);
    };
    /*
      login(user: User){
        var authenticatedUser = users.find(u => u.email === user.email);
        if (authenticatedUser && authenticatedUser.password === user.password){
          localStorage.setItem("user", JSON.stringify(authenticatedUser));
          this._router.navigate(['home']);
          return true;
        }
        return false;
      }
    */
    AuthenticationService.prototype.login = function (_LoginUser_Binding_VM) {
        var jsonString_LoginUser_Binding_VM = JSON.stringify(_LoginUser_Binding_VM);
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        var options = new http_1.RequestOptions({ headers: headers, method: "post" });
        return this.http.post('http://localhost:1479/api/account/UserLogin', jsonString_LoginUser_Binding_VM, options)
            .map(function (response) {
            console.debug("response.json()" + response.json());
        })
            .subscribe();
    };
    AuthenticationService.prototype.checkCredentials = function () {
        if (localStorage.getItem("user") === null) {
            this._router.navigate(['login']);
        }
    };
    return AuthenticationService;
}());
AuthenticationService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http,
        router_1.Router])
], AuthenticationService);
exports.AuthenticationService = AuthenticationService;
//# sourceMappingURL=authentication.service.js.map