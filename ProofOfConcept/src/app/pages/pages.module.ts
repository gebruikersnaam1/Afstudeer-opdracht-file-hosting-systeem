import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing.module';
import { HomeComponent } from './home/home.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { UserComponent } from './user/user.component';


@NgModule({
  declarations: [HomeComponent, NotfoundComponent, ServerErrorComponent, UserComponent],
  imports: [
    CommonModule,
    PagesRoutingModule
  ]
})
export class PagesModule { }
