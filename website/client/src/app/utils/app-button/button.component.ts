import { Component, OnInit, Input, Output, EventEmitter, HostListener } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent implements OnInit {

  @Input() id: string;
	@Input() label: string;
	@Input() route: string;
  @Input() href: string;
	@Input() class: string;
	@Input() ariaLabel: string;
	@Input() newWindow: boolean;
  @Input() customSfx: boolean;
  @Input() clickData: any;

  @Output() clickEvent = new EventEmitter<any>();

  @HostListener('click', ['$event.target'])
  onClick(targetElement) {
    
  }

  constructor() { }

  ngOnInit() {
  	
  }

  clickHandler(data: any) {

    this.clickEvent.emit(data);

  }

}
