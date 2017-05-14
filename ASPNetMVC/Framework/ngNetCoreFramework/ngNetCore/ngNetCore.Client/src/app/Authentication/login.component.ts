import {Component} from '@angular/core';
import {AuthenticationService, LoginUser_Binding_VM} from './authentication.service';
 
@Component({
    selector: 'login-form',
    providers: [AuthenticationService],
    templateUrl: './login.component.html'
})
 
export class LoginComponent {
 
    public _LoginUser_Binding_VM = new LoginUser_Binding_VM('','');
    public DiplayMessage = '';
 
    constructor(
        private _service:AuthenticationService) {}
 
    login() {
        /*
        if(!this._service.login(this._LoginUser_Binding_VM)){
            this.DiplayMessage = 'Failed to login';
        }
        */

        /// need to implement observable soon.
        var returnedValue = this._service.login(this._LoginUser_Binding_VM);
        //console.log("[LoginComponent] returnedValue = ", LoginComponent);

    }
}