import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'auth-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {


  constructor(private auth: AuthService) { }

  ngOnInit(): void {}

  onSubmit(){
     return this.auth.loginWithRedirect( { screen_hint: 'Inloggen' });
  }

}
