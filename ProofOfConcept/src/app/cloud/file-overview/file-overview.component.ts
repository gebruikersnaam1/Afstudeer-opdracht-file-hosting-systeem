import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CloudService } from "../shared/cloud.service";

import { folderView, FolderResponse, Folder } from '../interfaces/folder';

@Component({
  selector: 'cloud-overview',
  templateUrl: './file-overview.component.html',
  styleUrls: ['./file-overview.component.scss']
})
export class FileOverviewComponent implements OnInit {
  folder : Folder;
  rows : FolderResponse[];
  filesLoaded: Promise<boolean>;
  errorExist = false;
  
  folderData : folderView = {
    currentfolderID : 1, //root ID
    totalItems : 0,
    itemsPerPages : 10
  }
  

  constructor(private cloudService: CloudService, private activeRoute : ActivatedRoute, private router : Router) { 

  }

  searchForFile(searchTerm){
    if(searchTerm == "" || searchTerm === undefined){
      this.setCurrentPage();
      return;
    }
    this.cloudService.searchInFolders(searchTerm.toString()).subscribe(
      files => { this.rows = files; this.filesLoaded = Promise.resolve(true) },
      _ => { this.rows = []; this.filesLoaded = Promise.resolve(true)}
    );
  }

  onFolderRemoved(event : boolean){
      if(event == true){ 
        this.changeFolder(1);
      }
  }

  goFolderBack(){
    if(this.folderData.currentfolderID == 1 || this.folder.parentFolder == null){ //as 1 is his root
      return; 
    }
    this.changeFolder(this.folder.parentFolder.folderId);
  }

  //pagination
  changePage(pageNumber:number){
    ///not sure if needed anymore, for now empty method
  }

  changeFolder(folderId : number){
    this.folderData.currentfolderID = folderId > 0 ? folderId : 0;
    this.setFiles();
  }

  setCurrentPage(){
    this.activeRoute.params.subscribe( 
      (value) => {
        if(value?.folderID != undefined && Number(value?.folderID) != NaN){
          this.folderData.currentfolderID = value.folderID;
        }
        if(value?.pageNumber != undefined && Number(value?.pageNumber) != NaN){
          this.folderData.currentfolderID = value.pageNumber;
        }
      }
    );
    this.setFiles();
  }

  setFolder(){
    this.cloudService.getFolder(this.folderData.currentfolderID).subscribe((f: Folder) => this.folder = f);
  }

  setFiles(){
    this.setFolder();
    this.cloudService.getFolderContent(this.folderData.currentfolderID).subscribe(
      files => { this.rows = files; this.filesLoaded = Promise.resolve(true); },
      _ => { this.errorExist = true; this.filesLoaded = Promise.resolve(false); }
    );
  }

  ngOnInit(): void {
    this.setCurrentPage();
  }


}
