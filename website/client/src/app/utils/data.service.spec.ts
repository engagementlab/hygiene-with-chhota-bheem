
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed, inject } from '@angular/core/testing';
import { DataService } from './data.service';
import { HttpClient } from 'selenium-webdriver/http';

describe('DataService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [DataService],
    imports: [
      HttpClient,
      HttpClientTestingModule
    ]
  }));

/*   it('expects service to fetch mock data', inject([HttpTestingController, DataService],
    (httpMock: HttpTestingController, service: DataService) => {

    service.getDataForUrl('homepage/get/').subscribe(response => {
      expect(response).toBeDefined();
    });
          // We set the expectations for the HttpClient mock
          const req = httpMock.expectOne('http://.../data/contacts');
          expect(req.request.method).toEqual('GET');
    
          // Then we set the fake data to be returned by the mock
          req.flush({data: ...});
    })); */
});
