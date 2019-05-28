import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { filter } from 'rxjs/operators';

import { environment } from '../environments/environment';
import { DataService } from './utils/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, AfterViewInit {

  public isQABuild: boolean;
  public currentLang: string;

  title = 'Hygiene With Chhota Bheem';

  private currentUrl: string;

  constructor(private _router: Router, private _route: ActivatedRoute, private _titleSvc: Title, private _dataSvc: DataService) {

    this.isQABuild = environment.qa;
    this._titleSvc.setTitle((this.isQABuild ? '(QA) ' : '') + this.title);

    // Get nav route when nav ends
    _router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(e => {
      this.currentUrl = _router.url;
    });

    this.currentLang = this._dataSvc.currentLang.value;
    this._dataSvc.currentLang.subscribe((val) => {
      this.currentLang = val;
    });
   
   }
 
  ngOnInit() {
    
    this._router.events.subscribe((evt) => {

      if(evt instanceof NavigationEnd) {

        // Close menu
        document.getElementById('body').classList.remove('open');
        document.getElementById('menu').classList.remove('open');
        document.getElementById('menu-btn').classList.remove('open');
        
      }

      if (!(evt instanceof NavigationEnd))
        return;
    
      // Always go to top of page
      window.scrollTo(0, 0);

    });
    
  }

  ngAfterViewInit() {

    // Catch language change
    this._route.queryParams.subscribe(params => {
    
      if(params['tm'] !== undefined) 
        this._dataSvc.currentLang.next('tm');
      else if(params['hi'] !== undefined) 
          this._dataSvc.currentLang.next('hi');
        
    });

  }

  // Is passed route active?
  itemActive(route: string) {

    return '/'+route == this.currentUrl;

  }
}
