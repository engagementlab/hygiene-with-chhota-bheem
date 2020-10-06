import { Component, OnInit } from '@angular/core';
import { DataService } from '../utils/data.service';

import * as AOS from 'aos';
import isMobile from 'ismobilejs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  public content: any;
  public isPhone: boolean;
  public videoPlay: boolean;
  public hasContent: boolean;

  public videoUrl: SafeResourceUrl;

  constructor(private _dataSvc: DataService, private _santizer: DomSanitizer) {
    this.isPhone = isMobile(window.navigator.userAgent).phone;
  }

  async ngOnInit() {

    const response = await this._dataSvc.getDataForUrl('about/get/');
      this.content = response;
      this.hasContent = true;

  }

  ngAfterViewInit() {

    AOS.init({
      duration: 700,
      easing: 'ease-in-out'
    });

  }

  public showVideo() {

   this.videoUrl = this._santizer.bypassSecurityTrustResourceUrl('https://player.vimeo.com/video/' + this.content.videoId + '?color=4a90e2&byline=0&portrait=0');
   this.videoPlay = true;

  }
}
