import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../data.service';

@Component({
  selector: 'app-localized',
  templateUrl: './localized.component.html',
  styleUrls: ['./localized.component.scss']
})
export class LocalizedComponent implements OnInit {

	@Input() en: string;
  @Input() tm: string;

  public showEn: boolean;
  
  constructor(private _dataSvc: DataService) {
    
    this.showEn = this._dataSvc.currentLang.value === 'en';
    this._dataSvc.currentLang.subscribe((val) => {
      this.showEn = val === 'en'; 
    });
    
   }

  ngOnInit() {
  }

}
