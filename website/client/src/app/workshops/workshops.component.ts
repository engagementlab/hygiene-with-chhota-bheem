import { Component, OnInit, AfterViewInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as AOS from 'aos';

@Component({
  selector: 'app-workshops',
  templateUrl: './workshops.component.html',
  styleUrls: ['./workshops.component.scss']
})
export class WorkshopsComponent implements OnInit, AfterViewInit {

  public content: any;
  public isPhone: boolean;
  public hasContent: boolean;

  constructor(private _dataSvc: DataService) { }

  ngOnInit() {

    this._dataSvc.getDataForUrl('workshops/get/').subscribe(response => {

      this.content = response;
      this.hasContent = true;

      this.content['oneDayfacGuide'] = this.content['oneDayfacGuideEn'] || this.content['oneDayfacGuideTm'] || this.content['oneDayfacGuideHi'];
      this.content['fourDayfacGuide'] = this.content['fourDayfacGuideEn'] || this.content['fourDayfacGuideTm'] || this.content['fourDayfacGuideHi'];
      
      this.content['story1'] = this.content['story1En'] || this.content['story1Tm'] || this.content['story1In'];
      this.content['story2'] = this.content['story2En'] || this.content['story2Tm'] || this.content['story2In'];

    });

  }

  ngAfterViewInit() {

    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

  }

}
