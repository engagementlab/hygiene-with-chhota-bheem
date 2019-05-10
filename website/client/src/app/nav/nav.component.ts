import { Component } from '@angular/core';
import { DataService } from '../utils/data.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  public showEn: boolean = true;

  constructor(private _dataSvc: DataService) {
 
    this.showEn = this._dataSvc.currentLang.value === undefined;
    this._dataSvc.currentLang.subscribe((val) => {
      this.showEn = val === undefined; 
    });
  
  }


  openCloseNav() {

    document.getElementById('body').classList.toggle('open');
    document.getElementById('menu').classList.toggle('open');
    
  }

}
