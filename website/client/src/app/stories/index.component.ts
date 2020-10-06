import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as AOS from 'aos';
import isMobile from 'ismobilejs';

@Component({
  selector: 'app-stories-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class StoryIndexComponent implements OnInit {
  
  public content: unknown;
  public hasContent: boolean;
  public isPhone: boolean;
  
  constructor(private _dataSvc: DataService) {
    
    this.isPhone = isMobile(window.navigator.userAgent).phone;
    
  }
  
  async ngOnInit() {
    
    const response = await this._dataSvc.getDataForUrl('stories/get/');
      
      this.content = response;
      this.hasContent = true;
    

  }

  ngAfterViewInit() {

    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

  }
}
