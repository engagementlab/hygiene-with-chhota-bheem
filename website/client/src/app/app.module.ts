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
import { InitiativeComponent } from './initiatives/initiative.component';
import { AboutComponent } from './about/about.component';

import { EventIndexComponent } from './events/index.component';
import { EventComponent } from './events/event.component';

import { ProjectIndexComponent } from './projects/index.component';
import { ProjectComponent } from './projects/project.component';

import { PublicationIndexComponent } from './publications/index.component';
import { PublicationComponent } from './publications/publication.component';

import { TeamComponent } from './team/team.component';
import { PeopleGridComponent } from './team/people-grid.component';
import { ContactComponent } from './contact/contact.component';
import { MastersComponent } from './masters/masters.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { JobsComponent } from './jobs/jobs.component';
import { RedirectComponent } from './redirect/redirect.component';

import { DataService } from './utils/data.service';
import { RedirectService } from './utils/redirect.service';
import { AuthorFormatPipe } from './utils/author-format.pipe';

export const cloudinary = {
  Cloudinary: CloudinaryCore
};
export const config: CloudinaryConfiguration = cloudinaryConfiguration;

// App routes
export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'initiatives/:key', component: InitiativeComponent },
  { path: 'about', component: AboutComponent },  
  
  { path: 'team', component: TeamComponent },  
  { path: 'team/:key', component: TeamComponent },  
  // Alias
  { path: 'people', component: TeamComponent },  
  { path: 'people/:key', component: TeamComponent },  
  
  { path: 'projects', component: ProjectIndexComponent },
  { path: 'projects/:key', component: ProjectComponent },
  // Support old URL struct
  { path: 'projects/:category/:key', component: ProjectComponent },

  { path: 'events', component: EventIndexComponent },
  { path: 'events/:key', component: EventComponent },

  { path: 'publications', component: PublicationIndexComponent },
  
  { path: 'getinvolved', component: ContactComponent },

  { path: 'cmap', component: MastersComponent },
  { path: 'masters', component: MastersComponent },

  { path: 'contact', component: ContactComponent },
  { path: 'press', component: ContactComponent },

  { path: 'privacy', component: PrivacyComponent },
  { path: 'jobs', component: JobsComponent },
  
  { path: 'redirect', component: RedirectComponent, canActivate:[RedirectService] },

  { path: 'pokemon', component: RedirectComponent, canActivate:[RedirectService], data: {
      externalUrl: 'https://www.launchpad6.com/contestpad'
    }
  },

/*   , component: RedirectComponent, canActivate:[RedirectService], data: {
      externalUrl: 'https://www.emerson.edu/academics/media-design-ma'
    } 
  } */

];

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ProjectIndexComponent,
    ProjectComponent,
    NavComponent,
    FooterComponent,
    CdnImageComponent,
    ButtonComponent,
    PrettyUrlPipe,
    AboutComponent,
    TeamComponent,
    RedirectComponent,
    PeopleGridComponent,
    PublicationIndexComponent,
    AuthorFormatPipe,
    PublicationComponent,
    ContactComponent,
    PrivacyComponent,
    MastersComponent,
    JobsComponent,
    EventIndexComponent,
    EventComponent,
    InitiativeComponent
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
