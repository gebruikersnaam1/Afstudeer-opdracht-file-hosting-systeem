import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AuthGuard } from './auth/shared/auth.guard';
import { PublicFileComponent } from './cloud/public-file/public-file.component';

const routes: Routes = [
  { path: 'auth', loadChildren: () => import("./auth/auth.module").then(m => m.AuthModule)},
  { path: 'cloud/public/:id', component: PublicFileComponent },
  { path: 'cloud', canLoad: [AuthGuard], loadChildren: () =>  import("./cloud/cloud.module").then(m => m.CloudModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
