import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer, SafeStyle } from '@angular/platform-browser';

@Component({
  selector: 'cdn-image',
  templateUrl: './cdn-image.component.html',
  styleUrls: ['./cdn-image.component.scss']
})
export class CdnImageComponent implements OnInit {

	@Input() cloudinaryId: string;
	@Input() cloudinaryPrefix: string;
	@Input() alt: string;
  @Input() effect: string = 'brightness:0';
  @Input() crop: string = 'scale';
  @Input() gravity: string = 'auto:none';
  @Input() height: number;
	@Input() width: string;
  @Input() quality: number;

  @Input() responsive: boolean = true;
	@Input() autoFormat: boolean = false;
	@Input() svg: boolean = false;

  public widthCss: SafeStyle;
  public widthAuto: SafeStyle;
  public imgId: string;

  constructor(private _sanitizer: DomSanitizer) {
  }
  
  ngOnInit() {

    this.imgId = (this.cloudinaryPrefix ? this.cloudinaryPrefix : 'homepage-2.0/') + this.cloudinaryId;

    if(this.width)
      this.widthCss = this._sanitizer.bypassSecurityTrustStyle('width:' + this.width + 'px; max-width:' + this.width+'px');

  }

}
