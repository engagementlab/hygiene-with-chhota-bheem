import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  private wasLoading: boolean = false;
  private currentUrl: string;

  constructor(private _router: Router) {

    // Get nav route when nav ends
    _router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(e => {
      this.currentUrl = _router.url;
    });
  
  }

  // Is passed route active?
  itemActive(route: string) {

    return '/'+route == this.currentUrl;

  }

}
