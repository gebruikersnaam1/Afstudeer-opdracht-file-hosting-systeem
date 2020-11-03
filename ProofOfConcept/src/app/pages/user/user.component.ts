import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../auth/shared/auth.service';
import { User } from '../../auth/interfaces/authService.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  user: User;
  filesLoaded: Promise<boolean>;

  constructor(private authService: AuthService, private router : Router) { }

  ngOnInit(): void {
    this.authService.getActiveUser().subscribe( 
      (user : User) => { this.user = user; this.filesLoaded = Promise.resolve(true); }, 
       _ => this.router.navigateByUrl("/500")
    );
  }

}
