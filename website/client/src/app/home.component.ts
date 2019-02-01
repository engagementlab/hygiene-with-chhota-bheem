import { Component, OnInit, ViewChildren, QueryList, ViewChild, ElementRef } from '@angular/core';
import { DataService } from './utils/data.service';
import { TweenLite, Sine, TimelineLite } from 'gsap';

import * as AOS from 'aos';
import * as _ from 'underscore';
import * as ismobile from 'ismobilejs';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public initiatives: any[];
  public featuredProjects: any[];
  public events: any[];
  public latestEvent: any;

  public isPhone: boolean;

  private tls: TimelineLite[];
  t: TweenLite;

  @ViewChildren('initiativeList') initiativeList: QueryList<any>;
  
  @ViewChild('blueBg') blueBg: ElementRef;
  @ViewChild('bluePattern') bluePattern: ElementRef;

  @ViewChild('redBg') redBg: ElementRef;
  @ViewChild('redPattern') redPattern: ElementRef;

  @ViewChild('yellowBg') yellowBg: ElementRef;
  @ViewChild('yellowPattern') yellowPattern: ElementRef;

  constructor(private _dataSvc: DataService) {
    this.isPhone = ismobile.phone;
  }

  ngOnInit() {

    this._dataSvc.getDataForUrl('homepage/get/').subscribe(response => {
     
      this.initiatives = response.initiatives;    
      this.featuredProjects = response.projects;    
      this.events = response.events;    

      this.latestEvent = response.events[response.events.length-1];

    });

  }

  ngAfterViewInit() {

    this.initiativeList.changes.subscribe(t => {
        // AOS.init();
    });
    
    this.tls = [
      new TimelineLite({repeat:-1, yoyo:true}), 
      new TimelineLite({repeat:-1, yoyo:true}),
      new TimelineLite({repeat:-1, yoyo:true}),
      new TimelineLite({repeat:-1, yoyo:true}),
      new TimelineLite({repeat:-1, yoyo:true}),
      new TimelineLite({repeat:-1, yoyo:true})
    ];
    
    let i = 0;
    while(i < 20) {
      
      this.tls[0].add(TweenLite.to(this.blueBg.nativeElement, 8, { xPercent:_.random(-10, 10), yPercent:_.random(-10, 10), ease:Sine.easeInOut}));
      this.tls[1].add(TweenLite.to(this.bluePattern.nativeElement, 5.7, { xPercent:_.random(-15, 15), yPercent:_.random(-15, 15), ease:Sine.easeInOut}));

      this.tls[2].add(TweenLite.to(this.yellowBg.nativeElement, 8, { xPercent:_.random(-15, 15), yPercent:_.random(-25, 25), ease:Sine.easeInOut}));
      this.tls[3].add(TweenLite.to(this.yellowPattern.nativeElement, 5.7, { xPercent:_.random(-10, 10), yPercent:_.random(-25, 25), ease:Sine.easeInOut}));

      this.tls[4].add(TweenLite.to(this.redBg.nativeElement, 8, { xPercent:_.random(-7, 7), yPercent:_.random(-15, 15), ease:Sine.easeInOut}));
      this.tls[5].add(TweenLite.to(this.redPattern.nativeElement, 5.7, { xPercent:_.random(-5, 5), yPercent:_.random(-10, 10), ease:Sine.easeInOut}));

      i++;
    }

/*     anime({
      targets: '#red .path1',
      d: [
        { value: 'M 130.174 75 C 130.174 75 92.525 109.315 94.886 116.741 C 97.248 124.166 151.747 125.699 151.747 125.699 C 151.747 125.699 166.121 107.101 177.551 108.274 C 188.982 109.446 207.235 94.184 195.329 85.467 C 183.422 76.75 144.423 60.845 130.174 75 Z M 200 127.486 C 200 127.486 207.705 131.159 220.934 144.475 C 234.163 157.791 264.466 146.222 257.28 139.263 C 250.094 132.304 235.706 132.227 232.812 120.584 C 229.918 108.941 207.285 99.675 200 127.486 Z' }
      ],
      easing: 'easeOutQuad',

    direction: 'alternate',
      duration: 2000,
      loop: true
    }); */


  }

}
