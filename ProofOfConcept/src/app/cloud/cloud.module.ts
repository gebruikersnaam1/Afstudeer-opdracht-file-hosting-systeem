import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CloudRoutingModule } from './cloud-routing.module';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { ShowFileComponent } from './show-file/show-file.component';
import { FileOverviewComponent } from './file-overview/file-overview.component';

import { SharedModule } from '../shared/shared.module';
import { FileTableComponent } from './shared/file-table/file-table.component';

import { ReactiveFormsModule } from '@angular/forms';
import { CreatefolderComponent } from './createfolder/createfolder.component';

@NgModule({
  declarations: [FileUploadComponent, ShowFileComponent, FileOverviewComponent, FileTableComponent, CreatefolderComponent],
  imports: [
    CommonModule,
    CloudRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ]
})
export class CloudModule { }
