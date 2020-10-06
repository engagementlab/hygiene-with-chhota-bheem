import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';

import isMobile from 'ismobilejs';

@Component({
  selector: 'cdn-image',
  templateUrl: './cdn-image.component.html',
  styleUrls: ['./cdn-image.component.scss'],
  exportAs: 'cdn-image'
})
export class CdnImageComponent implements OnInit {

	@Input() cloudinaryId: string;
	@Input() cloudinaryPrefix: string;
	@Input() alt: string;
  @Input() effect: string = 'brightness:0';
  @Input() crop: string = 'scale';
  @Input() gravity: string = 'auto:none';
  @Input() height: string;
	@Input() width: string = 'auto';
	@Input() phoneWidth: string;
  @Input() quality: number;

  @Input() responsive: boolean = true;
	@Input() autoFormat: boolean = false;
	@Input() svg: boolean = false;

  public widthCss: SafeStyle;
  public widthAuto: SafeStyle;
  public imgId: string;
  public isPhone: boolean;

  constructor(private _sanitizer: DomSanitizer) {

    this.isPhone = isMobile(window.navigator.userAgent).phone;

  }
  
  ngOnInit() {

    this.imgId = (this.cloudinaryPrefix ? this.cloudinaryPrefix : 'chhota-bheem/') + this.cloudinaryId;
    let useMobileWidth = (this.isPhone && this.phoneWidth);

    if(useMobileWidth)
      this.widthCss = this._sanitizer.bypassSecurityTrustStyle('width:' + this.phoneWidth + 'px; max-width:' + this.phoneWidth+'px');
    else if(this.width)
      this.widthCss = this._sanitizer.bypassSecurityTrustStyle('width:' + this.width + 'px; max-width:' + this.width+'px')
  }

}
