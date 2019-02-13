import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as AOS from 'aos';
import * as ismobile from 'ismobilejs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss']
})
export class StoryComponent implements OnInit {
  
  public content: any;
  public prev: any;
  public next: any;

  public hasContent: boolean;
  public isPhone: boolean;
  
  constructor(private _dataSvc: DataService, private _route: ActivatedRoute) {
    
    this.isPhone = ismobile.phone;
    
  }
  
  ngOnInit() {
    

    this._route.params.subscribe(params => {
      this._dataSvc.getDataForUrl('stories/get/' + params['key']).subscribe(response => {
        
        this.content = response.person;
        this.next = response.next;
        this.prev = response.prev;
        this.hasContent = true;
        
      });
    });

  }

  ngAfterViewInit() {

    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

  }

}
