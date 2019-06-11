import { Component } from '@angular/core';
import { DataService } from '../utils/data.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  public currentLang: string;

  constructor(private _dataSvc: DataService) {

    this.currentLang = this._dataSvc.currentLang.value;
    this._dataSvc.currentLang.subscribe((val) => {
      this.currentLang = val;
    });
  
  }


  openCloseNav() {

    document.getElementById('body').classList.toggle('open');
    document.getElementById('menu').classList.toggle('open');
    document.getElementById('menu-btn').classList.toggle('open');
    
  }

}
