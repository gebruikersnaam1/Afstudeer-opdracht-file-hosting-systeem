import { Injectable } from '@angular/core';
import { CanActivate, CanLoad, Route, UrlSegment, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { skipWhile, take, tap } from 'rxjs/operators';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanLoad {

  constructor(private authService :AuthService, private router : Router){}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }
  
  canLoad(
    route: Route,
    segments: UrlSegment[]): Observable<boolean> | Promise<boolean> | boolean {
      return this.authService.signedIn$.pipe(
        skipWhile(value => value === null),
        take(1),
          tap(result=>{
            if(!result){
              this.router.navigateByUrl('/auth/login');
            }
          })
      );
  }
}
