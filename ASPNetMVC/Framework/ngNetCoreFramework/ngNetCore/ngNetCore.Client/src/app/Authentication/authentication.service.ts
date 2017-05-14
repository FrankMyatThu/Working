import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
 
export class LoginUser_Binding_VM {
  constructor(    
    public UserName: string,
    public Password: string) { }
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

  login(_LoginUser_Binding_VM: LoginUser_Binding_VM) {
      var jsonString_LoginUser_Binding_VM = JSON.stringify(_LoginUser_Binding_VM);      
      let headers = new Headers({'Content-Type':'application/json'});
      let options = new RequestOptions({ headers: headers, method: "post", withCredentials: true });
      return this.http.post('http://localhost:1479/api/account/UserLogin', jsonString_LoginUser_Binding_VM, options)
            .map((response: Response) => {                
                console.log("response.json() = " + JSON.stringify(response.json()));
                return JSON.stringify(response.json());
                /*
                var retrunedJson = response.json();                
                if(retrunedJson.isOk == true){
                    alert("login success.");
                }else{
                    alert("not ok");
                }
                */
                /// if(json.isOk == "true") { redirect("Home.html");  } else { display error message; }

            })
            .subscribe();
    }

  checkCredentials(){
    if (localStorage.getItem("user") === null){
      this._router.navigate(['login']);
    }
  }
  
}