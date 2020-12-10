import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { PagesModule } from './pages/pages.module';

import { AuthHttpInterceptor, HttpMethod } from '@auth0/auth0-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthModule } from '@auth0/auth0-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    PagesModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: 'woefie.eu.auth0.com',
      clientId: 'd4IxY01gbJwb5rEYSnKNlcvdRXzgpMWi',
      audience: 'https://woefie.eu.auth0.com/api/v2/',
      scope: 'read:users',
      redirectUri: window.location.origin,
        httpInterceptor: {
          allowedList: [
      
           // Using an absolute URIhttps://woefie.eu.auth0.com/api/v2/
            {
              uri: 'https://localhost:44368/*',
              tokenOptions: {
                audience: 'https://woefie.eu.auth0.com/api/v2/',
                scope: 'read:users',
              },
            }
          ],
        },
      }),
    BrowserAnimationsModule
  ],
  providers: [
    // { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
