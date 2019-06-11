import { Component, OnInit } from '@angular/core';
import { DataService } from './utils/data.service';

import { ScrollToService } from '@nicky-lenaers/ngx-scroll-to';

import * as AOS from 'aos';
import * as _ from 'underscore';
import * as ismobile from 'ismobilejs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public modules: any[];
  public files: any;
  public isPhone: boolean;
  public currentLang: string;
  
  constructor(private _dataSvc: DataService, private _scrollToSvc: ScrollToService) {
    this.isPhone = ismobile.phone;    
  }

  ngOnInit() {

    this.currentLang = this._dataSvc.currentLang.value;
    this._dataSvc.currentLang.subscribe((val) => {
      this.currentLang = val;
    });

    this._dataSvc.getDataForUrl('homepage/get/').subscribe(response => {

      this.modules = response.content.reverse();
      this.files = response.files;

    });

  }

  ngAfterViewInit() {

    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

  }

  goToModule(moduleNum: number) {

    this._scrollToSvc
      .scrollTo({
        target: document.getElementById('module' + moduleNum),
        offset: 200,
        easing: 'easeOutQuint',
        duration: 700
      });

  }

}
