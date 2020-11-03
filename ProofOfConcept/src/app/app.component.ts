import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from './auth/shared/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  signedIn$ : BehaviorSubject<Boolean>;

  constructor(private authService: AuthService ){
    this.signedIn$ = this.authService.signedIn$;
  }

  ngOnInit(){
    this.authService.isAuthenticated().subscribe();
  }
}
