import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as AOS from 'aos';
import * as ismobile from 'ismobilejs';

@Component({
  selector: 'app-stories-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class StoryIndexComponent implements OnInit {
  
  public content: any[];
  public hasContent: boolean;
  public isPhone: boolean;
  
  constructor(private _dataSvc: DataService) {
    
    this.isPhone = ismobile.phone;
    
  }
  
  ngOnInit() {
    
    this._dataSvc.getDataForUrl('stories/get/').subscribe(response => {
      
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
