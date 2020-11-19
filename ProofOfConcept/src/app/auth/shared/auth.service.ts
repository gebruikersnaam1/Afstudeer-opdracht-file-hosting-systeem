import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthService as Auth0Service} from '@auth0/auth0-angular';
import { tap } from 'rxjs/operators';
import { User } from '../interfaces/authService.interface';



@Injectable({
  providedIn: 'root'
})
export class AuthService {
  signedIn$ = new BehaviorSubject<boolean>(null);
  
  constructor(private auth0Service : Auth0Service) { }

  getActiveUser = () : Observable<User> =>{
    return this.auth0Service.user$;
  }

  getToken = ()  => {
    return this.auth0Service.getAccessTokenSilently();
  }


  isAuthenticated = () =>{
    return this.auth0Service.isAuthenticated$.pipe(tap(z => 
      {
        this.signedIn$.next(z);  
      }
    ));
  }
}
