import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
 
export class LoginUser_Binding_VM {
  constructor(    
    public UserName: string,
    public Password: string) { }
}
 
var LoginUser_Binding_VMs = [
  new LoginUser_Binding_VM('admin@admin.com','adm9'),
  new LoginUser_Binding_VM('user1@gmail.com','a23')
];
 
@Injectable()
export class AuthenticationService {
 
  constructor(
    private http: Http,
    private _router: Router){}

  logout() {
    localStorage.removeItem("user");
    this._router.navigate(['login']);
  }

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

  login(_LoginUser_Binding_VM: LoginUser_Binding_VM) {
      var jsonString_LoginUser_Binding_VM = JSON.stringify(_LoginUser_Binding_VM);      
      let headers = new Headers({'Content-Type':'application/json'});
      let options = new RequestOptions({ headers: headers, method: "post", withCredentials: true });
      return this.http.post('http://localhost:1479/api/account/UserLogin', jsonString_LoginUser_Binding_VM, options)
            .map((response: Response) => {                
                console.log("response.json()" + JSON.stringify(response.json()));
            })
            .subscribe();
    }

  checkCredentials(){
    if (localStorage.getItem("user") === null){
      this._router.navigate(['login']);
    }
  }
  
}