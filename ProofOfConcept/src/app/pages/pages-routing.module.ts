import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent} from './home/home.component';
import { NotfoundComponent } from './notfound/notfound.component'; 
import { ServerErrorComponent } from './server-error/server-error.component';
import { UserComponent } from './user/user.component';

import { AuthGuard} from '../auth/shared/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  { path: 'user', canLoad: [AuthGuard], component:UserComponent },
  {path: '404', component: NotfoundComponent},
  {path: '500', component: ServerErrorComponent},
  {path: '**', redirectTo: '/404'}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
