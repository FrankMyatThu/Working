import {Component} from '@angular/core';
import {AuthenticationService, LoginUser_Binding_VM, Token_Message_VM} from './authentication.service';

 
@Component({
    selector: 'login-form',
    providers: [AuthenticationService],
    templateUrl: './login.component.html'
})
 
export class LoginComponent {
 
    public _LoginUser_Binding_VM = new LoginUser_Binding_VM('','');
    public DiplayMessage = '';
    private _Token_Message_VM : Token_Message_VM; 
 
    constructor(
        private _service:AuthenticationService) {}
 
    login() {
        /*
        if(!this._service.login(this._LoginUser_Binding_VM)){
            this.DiplayMessage = 'Failed to login';
        }
        */

        /// need to implement observable soon.
        this._service.login(this._LoginUser_Binding_VM).subscribe(
                function(response) { 
                    console.log("Success Response" + response);
                    this._Token_Message_VM = response;
                    console.log("stringify() = ", JSON.stringify(this._Token_Message_VM));                                  
					if(this._Token_Message_VM.isOk == true){
						alert("login success....");
					}else{
						alert("not ok");
					}

                },
                function(error) { console.log("Error happened" + error)},
                function() { console.log("the subscription is completed")}
        );



        //console.log("[LoginComponent] returnedValue = ", LoginComponent);



    }
}