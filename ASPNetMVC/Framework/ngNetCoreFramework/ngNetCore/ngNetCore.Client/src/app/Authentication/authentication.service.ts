import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {Router} from '@angular/router';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
 
export class LoginUser_Binding_VM {
  constructor(    
    public UserName: string,
    public Password: string) { }
}

export class Token_Message_VM {
  constructor(    
    public JWTToken: string,
    public MessageType: string,
    public MessageDescription: string,
    public IsOk: boolean) { }
}
 
@Injectable()
export class AuthenticationService {
 
  constructor(
    private http: Http,
    private _router: Router){}

  logout() {
    //localStorage.removeItem("user");
    this._router.navigate(['login']);
  }

  login(_LoginUser_Binding_VM: LoginUser_Binding_VM) : Observable<Token_Message_VM> {
      var jsonString_LoginUser_Binding_VM = JSON.stringify(_LoginUser_Binding_VM);      
      let headers = new Headers({'Content-Type':'application/json'});
      let options = new RequestOptions({ headers: headers, method: "post", withCredentials: true });
      return this.http.post('http://localhost:1479/api/account/UserLogin', jsonString_LoginUser_Binding_VM, options)
            .map((response: Response) => { return response.json(); });            
    }

  checkCredentials(){
    if (localStorage.getItem("user") === null){
      this._router.navigate(['login']);
    }
  }
     
}