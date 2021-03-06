import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { InputComponent } from './input/input.component';
import { SearchbalkComponent } from './searchbalk/searchbalk.component';
import { PaginationComponent } from './pagination/pagination.component';

import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FileTypeIconPipe } from './pipes/file-type-icon.pipe';
import { ModalComponent } from './modal/modal.component';
import { BytesConverterPipe } from './pipes/bytes-converter.pipe';



@NgModule({
  declarations: [MenuComponent, InputComponent, SearchbalkComponent, PaginationComponent, FileTypeIconPipe, ModalComponent, BytesConverterPipe],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule
  ],
  exports: [MenuComponent, InputComponent, SearchbalkComponent, PaginationComponent, FileTypeIconPipe, ModalComponent, BytesConverterPipe]
})
export class SharedModule { }
