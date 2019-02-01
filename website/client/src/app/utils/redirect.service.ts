import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class RedirectService implements CanActivate {

  constructor(_router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

    setTimeout(() => {
      window.location.href = route.queryParams['u'];
    }, 4200);
    return true;

  }

}
