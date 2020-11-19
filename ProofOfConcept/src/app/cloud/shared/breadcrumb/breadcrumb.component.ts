import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {Folder } from '../../interfaces/folder';

@Component({
  selector: 'cloud-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {
  @Input() folder : Folder;
  @Output() folderChange = new EventEmitter<number>();
  pageLoaded: Promise<boolean>;
  
  folders : Folder[];
  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(){
    if(this.folder != undefined){
      this.folders = this.setFoldertreeIntoArray(this.folder).reverse();
    }
  }

  openFolderStructure(folderId: number){
    console.log(folderId);
  }

  changeFolder(folderId:number){
    this.folderChange.emit(folderId);
  }

  setFoldertreeIntoArray(folder : Folder) : Folder[]{
    if(folder.parentFolder == null){
      return [folder];
    }else{
      return [folder].concat(this.setFoldertreeIntoArray(folder.parentFolder));
    }
  }

}
