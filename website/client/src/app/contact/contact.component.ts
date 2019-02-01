import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';
import { ScrollToService } from '@nicky-lenaers/ngx-scroll-to';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.scss']
})
export class ContactComponent implements OnInit {

  public content: any;

  constructor(private _dataSvc: DataService, private _scrollToSvc: ScrollToService) { }

  ngOnInit() {

    this._dataSvc.getDataForUrl('contact/get/').subscribe(response => {
      
      this.content = response[0];

    });

  }
  
  public scrollToBottom() {
    this._scrollToSvc
      .scrollTo({
        target: document.getElementById('bottom'),
        offset: 0,
        easing: 'easeOutQuint',
        duration: 700
      });
  }
  
}
