import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthenticationGuard implements CanActivate {
  constructor(private router: Router)
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    if (localStorage.getItem('token') != null) {
      console.log('Access granted');
      return true;
    }
    else {
      console.log('Access denied!');
      this.router.navigate(['/user'])
      return false;
    }
  }
}
