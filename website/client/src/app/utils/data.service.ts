import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router, NavigationStart } from '@angular/router';

import { Subject, BehaviorSubject } from 'rxjs';
import { throwError } from 'rxjs';

import { environment } from '../../environments/environment';

import * as _ from 'underscore';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/observable/of';

import { isScullyGenerated, TransferStateService } from '@scullyio/ng-lib'

@Injectable()
export class DataService {

  public isLoading: Subject<boolean> = new Subject<boolean>();
  public serverProblem: Subject<boolean> = new Subject<boolean>();

  public previousUrl: string;
  public currentUrl: string;

  public currentLang: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  private baseUrl: string;

  constructor(private http: HttpClient, private _router: Router,
    private transferState: TransferStateService) { 

    this.currentLang.next(undefined);
    
  	this.baseUrl = 'https://elab.emerson.edu/hygiene/api/';

    _router.events.subscribe(event => {
      
      this.currentUrl = this._router.url;
      // Track prior url
      if (event instanceof NavigationStart) {
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
      }
      
    }); 
  }

	
  public getDataForUrl(urlParam: string, query: string = ''): Promise<unknown> {

    this.isLoading.next(true);
    this.serverProblem.next(false);

    let url = this.baseUrl + urlParam;
      
  //   if(this.currentLang.value === undefined)
  //   url += 'en'
  // else if(this.currentLang.value === 'tm')
  //   url += 'tm'
  // else if(this.currentLang.value === 'hi')
  //   url += 'hi'

    //  url += '?'+query;

    // If scully is building or dev build, cache data from content API in transferstate
    if (!isScullyGenerated()) {
        const content = new Promise < unknown > ((resolve, reject) => {


            this.http.get(`${url}hi?${query}`).toPromise().then(res => {
                // Cache hindi result in state
                this.transferState.setState(`${urlParam}hi?${query}`, res);

                this.http.get(`${url}tm?${query}`).toPromise().then(res => {
                    // Cache tamil result in state
                    this.transferState.setState(`${urlParam}tm?${query}`, res);

                    return this.http.get(url).toPromise().then(resEn => {
                            // Cache result in state
                            this.transferState.setState(urlParam + query + this.currentLang.value, resEn);
                            resolve(resEn['data']);
                            return resEn['data'];
                        })
                        .catch((error: any) => {
                            reject(error);
                            this.isLoading.next(false);
                            return throwError(error);
                        });

                });
            });
        });
        return content;
    } else {
      
      console.log('get state', `${urlParam}${this.currentLang.value}${query}`)
      // Get cached state for this key
      const state = new Promise<unknown[]>((resolve, reject) => {
        try {
          this.transferState
          .getState < unknown[] > (`${urlParam}${this.currentLang.value}?${query}`)
          .subscribe(res => {
            console.log('res',res)
            resolve(res['data'])
              return res['data'];
            });
        } catch (error) {
          this.isLoading.next(false);
        }
      });
      return state;
    }
  }
  
}
