import { Component, OnInit } from '@angular/core';
import { ScrollToService } from '@nicky-lenaers/ngx-scroll-to';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  constructor(private _scrollToSvc: ScrollToService) { }

  ngOnInit() {
  }
  
  public scrollToTop() {
    this._scrollToSvc
      .scrollTo({
        target: document.getElementById('nav'),
        offset: 0,
        easing: 'easeOutQuint',
        duration: 700
      });
  }
}
