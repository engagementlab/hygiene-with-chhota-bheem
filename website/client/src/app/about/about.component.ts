import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as ismobile from 'ismobilejs';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  public isPhone: boolean;
  public isTablet: boolean;
  public about: any;
  public partners: any[];
  public people: any[];

  constructor(private _dataSvc: DataService) { 
    
    this.isPhone = ismobile.phone;
    this.isTablet = ismobile.tablet;

   }

  ngOnInit() {

    // this._dataSvc.getDataForUrl('about/get/').subscribe(response => {
    // });

  }

}
