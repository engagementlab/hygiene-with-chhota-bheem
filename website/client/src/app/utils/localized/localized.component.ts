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
  @Input() hi: string;

  public currentLang: string;
  
  constructor(private _dataSvc: DataService) {
    
    this.currentLang = this._dataSvc.currentLang.value;
    this._dataSvc.currentLang.subscribe((val) => {
      this.currentLang = val;
    });
    
   }

  ngOnInit() {
  }

}
