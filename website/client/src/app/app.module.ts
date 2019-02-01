import { BrowserModule, Title } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http'; 

// Utils
import { CdnImageComponent } from './utils/cdn-image/cdn-image.component';
import { ButtonComponent } from './utils/app-button/button.component';
import { PrettyUrlPipe } from './utils/pretty-url.pipe';

// NPM
import { Cloudinary as CloudinaryCore } from 'cloudinary-core';
import { CloudinaryConfiguration, CloudinaryModule } from '@cloudinary/angular-5.x';
import cloudinaryConfiguration from './cdn.config';
import { ScrollToModule } from '@nicky-lenaers/ngx-scroll-to';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { NavComponent } from './nav/nav.component';
import { FooterComponent } from './footer/footer.component';

import { HomeComponent } from './home.component';
import { AboutComponent } from './about/about.component';

import { DataService } from './utils/data.service';
import { RedirectService } from './utils/redirect.service';

export const cloudinary = {
  Cloudinary: CloudinaryCore
};
export const config: CloudinaryConfiguration = cloudinaryConfiguration;

// App routes
export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    FooterComponent,
    
    HomeComponent,
    AboutComponent,
    CdnImageComponent,
    ButtonComponent,
    PrettyUrlPipe,
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    CloudinaryModule.forRoot(cloudinary, config),
    HttpClientModule,
    RouterModule.forRoot(routes),
    ScrollToModule.forRoot()
  ],
  providers: [
    DataService,
    RedirectService,
    Title
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
