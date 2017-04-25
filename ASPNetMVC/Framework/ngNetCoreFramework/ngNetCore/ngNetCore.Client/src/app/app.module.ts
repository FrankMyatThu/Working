import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { LoginComponent } from './Authentication/login.component';
import { PrivateComponent } from './Authentication/private.component';
import { FormtesterComponent } from './FormTester/formtester.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    PrivateComponent,
    FormtesterComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,    
    RoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
