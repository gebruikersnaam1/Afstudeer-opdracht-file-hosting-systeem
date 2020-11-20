import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CloudRoutingModule } from './cloud-routing.module';
import { FileUploadComponent } from './modal_components/file-upload/file-upload.component';
import { ShowFileComponent } from './show-file/show-file.component';
import { FileOverviewComponent } from './file-overview/file-overview.component';

import { SharedModule } from '../shared/shared.module';
import { FileTableComponent } from './shared/file-table/file-table.component';

import { ReactiveFormsModule } from '@angular/forms';
import { CreatefolderComponent } from './modal_components/createfolder/createfolder.component';
import { FolderManagementComponent } from './modal_components/folder-management/folder-management.component';
import { BreadcrumbComponent } from './shared/breadcrumb/breadcrumb.component';


import { FolderNavigationComponent } from './modal_components/folder-navigation/folder-navigation.component'; 


import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [FileUploadComponent, ShowFileComponent, FileOverviewComponent, FileTableComponent, CreatefolderComponent, FolderManagementComponent, BreadcrumbComponent, FolderNavigationComponent],
  imports: [
    CommonModule,
    CloudRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    MatTreeModule,
    MatIconModule, 
    MatButtonModule
  ]
})
export class CloudModule { }
