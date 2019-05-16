import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as AOS from 'aos';

@Component({
  selector: 'app-resources',
  templateUrl: './resources.component.html',
  styleUrls: ['./resources.component.scss']
})
export class ResourcesComponent implements OnInit {

  public content: any;
  public isPhone: boolean;
  public hasContent: boolean;

  constructor(private _dataSvc: DataService) { }

  ngOnInit() {
 
  this._dataSvc.getDataForUrl('resources/get/').subscribe(response => {

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
