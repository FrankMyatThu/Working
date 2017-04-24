import {Component} from '@angular/core';
import {AuthenticationService, LoginUser_Binding_VM} from './authentication.service';
 
@Component({
    selector: 'login-form',
    providers: [AuthenticationService],
    template: `
        <div class="container" >
            <div class="title">
                Welcome
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="input-field col s12">
                        <input [(ngModel)]="_LoginUser_Binding_VM.UserName" id="UserName" 
                            type="text" class="validate">
                        <label for="UserName">UserName</label>
                    </div>
                </div>
 
                <div class="row">
                    <div class="input-field col s12">
                        <input [(ngModel)]="_LoginUser_Binding_VM.Password" id="Password" 
                            type="password" class="validate">
                        <label for="Password">Password</label>
                    </div>
                </div>
 
                <span>{{errorMsg}}</span>
                <button (click)="login()" 
                    class="btn waves-effect waves-light" 
                    type="submit" name="action">Login</button>
            </div>
        </div>
    	`
})
 
export class LoginComponent {
 
    public _LoginUser_Binding_VM = new LoginUser_Binding_VM('','');
    public errorMsg = '';
 
    constructor(
        private _service:AuthenticationService) {}
 
    login() {
        if(!this._service.login(this._LoginUser_Binding_VM)){
            this.errorMsg = 'Failed to login';
        }
    }
}