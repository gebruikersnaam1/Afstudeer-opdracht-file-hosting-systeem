import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FileUploadComponent } from './file-upload/file-upload.component';
import { ShowFileComponent } from './show-file/show-file.component';
import { FileOverviewComponent } from './file-overview/file-overview.component';


const routes: Routes = [
  {path: 'file/upload', component:FileUploadComponent },
  {path: 'file/:fileId', component:ShowFileComponent },
  {
    path: '',children: [
        { path: 'overzicht', component: FileOverviewComponent },
        { path: 'overzicht/:pageNumber', component: FileOverviewComponent },
        { path: '', component: FileOverviewComponent }
      ]
  } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CloudRoutingModule { }
