import { Component, OnInit, ViewChildren, QueryList, ViewChild, ElementRef } from '@angular/core';
import { DataService } from './utils/data.service';
import { TweenLite, Sine, TimelineLite } from 'gsap';

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
  public isPhone: boolean;

  private tls: TimelineLite[];
  t: TweenLite;

  constructor(private _dataSvc: DataService) {
    this.isPhone = ismobile.phone;
  }

  ngOnInit() {

    this._dataSvc.getDataForUrl('homepage/get/').subscribe(response => {

      this.modules = response;

    });

  }

  ngAfterViewInit() {
    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

    // this.initiativeList.changes.subscribe(t => {
    // });

  }

}
