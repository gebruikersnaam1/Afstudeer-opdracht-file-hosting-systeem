import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AuthGuard } from './auth/shared/auth.guard';

const routes: Routes = [
  { path: 'auth', loadChildren: () => import("./auth/auth.module").then(m => m.AuthModule)},
  { path: 'cloud', canLoad: [AuthGuard], loadChildren: () =>  import("./cloud/cloud.module").then(m => m.CloudModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
