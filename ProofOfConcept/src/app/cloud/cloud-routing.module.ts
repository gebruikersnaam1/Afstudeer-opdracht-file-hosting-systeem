import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ShowFileComponent } from './show-file/show-file.component';
import { FileOverviewComponent } from './file-overview/file-overview.component';

const routes: Routes = [
  {path: 'file/:fileId', component:ShowFileComponent },
  {
    path: '',children: [
        { path: 'explorer', component: FileOverviewComponent },
        { path: 'explorer/:folderID', component: FileOverviewComponent },
        { path: 'explorer/:folderID/:pageNumber', component: FileOverviewComponent },
        { path: '', component: FileOverviewComponent }
      ]
  } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CloudRoutingModule { }
