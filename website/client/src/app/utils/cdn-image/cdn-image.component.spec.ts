import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CdnImageComponent } from './cdn-image.component';
import { CloudinaryImage, CloudinaryTransformationDirective } from '@cloudinary/angular-5.x';

describe('CdnImageComponent', () => {
  let component: CdnImageComponent;
  let fixture: ComponentFixture<CdnImageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CdnImageComponent, CloudinaryImage, CloudinaryTransformationDirective ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CdnImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  // it('should create', () => {
  //   expect(component).toBeTruthy();
  // });
});
