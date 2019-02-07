import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutComponent } from './about.component';
import { RedirectService } from '../utils/redirect.service';
import { CdnImageComponent } from '../utils/cdn-image/cdn-image.component';
import { CloudinaryImage, CloudinaryTransformationDirective } from '@cloudinary/angular-5.x';
import { DataService } from '../utils/data.service';
import { HttpClient } from '@angular/common/http';

describe('AboutComponent', () => {
  let component: AboutComponent;
  let fixture: ComponentFixture<AboutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutComponent, CdnImageComponent, CloudinaryImage, CloudinaryTransformationDirective ],
      providers: [ RedirectService, DataService ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
