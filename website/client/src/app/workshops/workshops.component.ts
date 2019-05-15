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

    });

  }

  ngAfterViewInit() {

    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

  }

}
