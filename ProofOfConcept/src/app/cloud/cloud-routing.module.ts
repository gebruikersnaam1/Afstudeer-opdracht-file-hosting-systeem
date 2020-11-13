import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { FileUploadComponent } from './file-upload/file-upload.component';
import { ShowFileComponent } from './show-file/show-file.component';
import { FileOverviewComponent } from './file-overview/file-overview.component';
import { CreatefolderComponent } from './createfolder/createfolder.component';

const routes: Routes = [
  {path: 'file/upload', component:FileUploadComponent },
  {path: 'folder/create/:id', component:CreatefolderComponent },
  {path: 'file/:fileId', component:ShowFileComponent },
  {
    path: '',children: [
        { path: 'overzicht', component: FileOverviewComponent },
        { path: 'overzicht/:folderID', component: FileOverviewComponent },
        { path: 'overzicht/:folderID/:pageNumber', component: FileOverviewComponent },
        { path: '', component: FileOverviewComponent }
      ]
  } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CloudRoutingModule { }
