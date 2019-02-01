import { Component, AfterViewInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { DataService } from '../utils/data.service';
import { filter } from 'rxjs/operators';

import { TimelineLite, Circ, Linear, TweenMax } from "gsap";

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements AfterViewInit {

  public navLinks: object[] = [
      {url: 'about', label: 'About'},
      {url: 'projects', label: 'Projects'},
      {url: 'publications', label: 'Publications'},
      {url: 'masters', label: 'Masters Program'},
      {url: 'getinvolved', label: 'Get Involved'}
  ];

  private tl: TimelineLite; 
  private btn: HTMLElement;
  
  private wasLoading: boolean = false;
  private currentUrl: string;

  constructor(private _router: Router, private _dataSvc: DataService) {

    // Get nav route when nav ends
    _router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(e => {
      this.currentUrl = _router.url;
    });

		this._dataSvc.isLoading.subscribe( value => {
      if(this.wasLoading && !value) {
        if(document.getElementById('menu-btn').classList.contains('open')) 
            this.tl.reverse();
          // this.tlLogo.pause(0);
      }
          
      this.wasLoading = value;
      if(value) {
        // this.tlLogo.play();
      }

  } );
  
  }

  ngAfterViewInit() {

  	let menu = document.getElementById('menu');
    let show = document.querySelector('#menu-btn .close');
    let hide = document.querySelector('#menu-btn .open');

    this.btn = document.getElementById('menu-btn');
   	this.tl = new TimelineLite({paused:true, reversed:true, onReverseComplete:() => {
      menu.querySelectorAll('h3 a').forEach((el: HTMLElement) => {
        el.classList.remove('visible');
      });
    }});
    
  	let tl = this.tl;

    tl.add('start');
    tl.set([document.getElementById('nav'), this.btn], {className:'+=open'}, 'start');
    
    tl.fromTo(show, .7, {xPercent:100, autoAlpha:0}, { xPercent:0, autoAlpha:1, ease:Circ.easeOut});
    tl.fromTo(hide, .7, {xPercent:0, autoAlpha:1}, { xPercent:100, autoAlpha:0, ease:Circ.easeOut}, '-=.7');

    tl.fromTo(menu, .7, {autoAlpha:0}, {autoAlpha:1, display:'flex', ease:Circ.easeOut}, '-=.7');
    tl.fromTo(document.getElementById('menu-overlay'), .5, {autoAlpha:0, display:'none'}, {autoAlpha:1, display:'block'}, '-=.7');

    tl.staggerFromTo(menu.querySelectorAll('h3'), .2, {autoAlpha:0, yPercent:-20}, {autoAlpha:1, yPercent:0}, .1, '-=.5', () => {
      menu.querySelectorAll('h3 a').forEach((el: HTMLElement) => {
        el.classList.add('visible');
      });
    });

  }

  openCloseNav() {

    if(!this.tl.reversed())
      this.tl.reverse().timeScale(1.3);
    else
      this.tl.play();

  }

  // Is passed route active?
  itemActive(route: string) {

    return '/'+route == this.currentUrl;

  }

  // If on home when logo clicked, just close menu
  logoClick() {

    if(this.currentUrl === '/')
      this.openCloseNav();

  }

}
