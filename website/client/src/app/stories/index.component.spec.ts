import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StoryIndexComponent } from './index.component';

describe('StoryIndexComponent', () => {
  let component: StoryIndexComponent;
  let fixture: ComponentFixture<StoryIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StoryIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StoryIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
