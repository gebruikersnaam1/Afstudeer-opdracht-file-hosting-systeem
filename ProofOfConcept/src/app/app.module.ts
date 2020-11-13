import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { PagesModule } from './pages/pages.module';

import { AuthHttpInterceptor, HttpMethod } from '@auth0/auth0-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthModule } from '@auth0/auth0-angular';

import { AuthInterceptor } from './auth/shared/auth.interceptor';

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
      redirectUri: window.location.origin,
        httpInterceptor: {
          allowedList: [
            // Attach access tokens to any calls to '/api' (exact match)
            '/api',
      
            // Attach access tokens to any calls that start with '/api/'
            '/api/*',
      
            // Match anything starting with /api/accounts, but also specify the audience and scope the attached
            // access token must have
            // {
            //   uri: '/api/users/*',
            //   tokenOptions: {
            //     audience: 'https://woefie.eu.auth0.com/',
            //     scope: 'read:users',
            //   },
            // },
      
            // Matching on HTTP method
            {
              uri: '/api/orders',
              httpMethod:  HttpMethod.Post,
              tokenOptions: {
                audience: 'https://woefie.eu.auth0.com/api/v2/',
                scope: 'write:orders',
              },
            },
      
           // Using an absolute URIhttps://woefie.eu.auth0.com/api/v2/
            {
              uri: 'https://woefie.eu.auth0.com/api/v2/users',
              tokenOptions: {
                audience: 'https://woefie.eu.auth0.com/api/v2/',
                scope: 'read:users',
              },
            },
          ],
        },
      })
  ],
  providers: [
    // { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
