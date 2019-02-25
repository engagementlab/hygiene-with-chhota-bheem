import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocalizedComponent } from './localized.component';

describe('LocalizedComponent', () => {
  let component: LocalizedComponent;
  let fixture: ComponentFixture<LocalizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocalizedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocalizedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
