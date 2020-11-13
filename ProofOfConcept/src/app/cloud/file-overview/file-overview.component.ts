import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CloudService } from "../shared/cloud.service";

import { folderView, FolderResponse } from '../interfaces/folder';

@Component({
  selector: 'cloud-overview',
  templateUrl: './file-overview.component.html',
  styleUrls: ['./file-overview.component.scss']
})
export class FileOverviewComponent implements OnInit {
  rows : FolderResponse[];
  filesLoaded: Promise<boolean>;
  errorExist = false;
  
  folderData : folderView = {
    currentfolderID : 1, //root ID
    totalItems : 0,
    itemsPerPages : 10
  }
  

 

  constructor(private cloudService: CloudService, private activeRoute : ActivatedRoute) { 
  }

  searchForFile(searchTerm){
    if(searchTerm == "" || searchTerm === undefined){
      this.setCurrentPage();
      return;
    }
    // this.cloudService.searchOnFileName(searchTerm.toString()).subscribe(
    //   files => { this.rows = files; this.filesLoaded = Promise.resolve(true) },
    //   _ => { this.rows = []; this.filesLoaded = Promise.resolve(true)}
    // );
  }

  updateCurrentPage(pageNumber : number){
    this.folderData.currentfolderID = pageNumber > 0 ? pageNumber : 0;
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

  setFiles(){
    this.cloudService.getFolder(this.folderData.currentfolderID).subscribe(
      files => { this.rows = files; this.filesLoaded = Promise.resolve(true) },
      _ => { this.errorExist = true; this.filesLoaded = Promise.resolve(false); }
    );
  }

  ngOnInit(): void {
    this.setCurrentPage();
  }


}
