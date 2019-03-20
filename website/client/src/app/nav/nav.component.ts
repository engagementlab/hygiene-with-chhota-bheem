import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { filter } from 'rxjs/operators';
import { DataService } from '../utils/data.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  public showEn: boolean = true;
  private currentUrl: string;

  constructor(private _router: Router, private _dataSvc: DataService) {

    // Get nav route when nav ends
    _router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(e => {
      this.currentUrl = _router.url;
    });
    
    this.showEn = this._dataSvc.currentLang.value === undefined;
    this._dataSvc.currentLang.subscribe((val) => {
      this.showEn = val === undefined; 
    });
  
  }

  // Is passed route active?
  itemActive(route: string) {

    return '/'+route == this.currentUrl;

  }

}
