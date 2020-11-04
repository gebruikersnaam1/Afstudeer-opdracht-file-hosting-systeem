import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CloudService } from "../shared/cloud.service";
import { fileData } from '../interfaces/file';

import { PageOverviewData } from '../interfaces/pageOverview';

@Component({
  selector: 'cloud-overview',
  templateUrl: './file-overview.component.html',
  styleUrls: ['./file-overview.component.scss']
})
export class FileOverviewComponent implements OnInit {
  headers = ['bestandsnaam', 'Upload datum', 'Uploader', "verwijderen"]
  rows : fileData[];
  filesLoaded: Promise<boolean>;
  errorExist = false;
  
  pageData : PageOverviewData = {
    totalItems : 0,
    itemsPerPages : 10,
    currentPage : 0
  }
  

 

  constructor(private cloudService: CloudService, private activeRoute : ActivatedRoute) { 
  }

  searchForFile(searchTerm){
    if(searchTerm == "" || searchTerm === undefined){
      this.setCurrentPage();
      return;
    }
    this.cloudService.searchOnFileName(searchTerm.toString()).subscribe(
      files => { this.rows = files; this.filesLoaded = Promise.resolve(true) },
      _ => { this.rows = []; this.filesLoaded = Promise.resolve(true)}
    );
  }

  updateCurrentPage(pageNumber : number){
    this.pageData.currentPage = pageNumber > 0 ? pageNumber : 0;
    this.setFiles();
  }

  setCurrentPage(){
    this.activeRoute.params.subscribe( (value) => 
    value?.pageNumber === undefined ? console.log("url/:pageNumber param doesn't exist!") : this.pageData.currentPage = value.pageNumber
    );
    this.setFiles();
  }

  setFiles(){
    this.cloudService.getFiles(this.pageData.itemsPerPages, this.pageData.currentPage).subscribe(
      files => { this.rows = files; this.filesLoaded = Promise.resolve(true) },
      _ => { this.errorExist = true; this.filesLoaded = Promise.resolve(false); }
    );
  }

  ngOnInit(): void {
    this.cloudService.getFileCount().subscribe(
      result => console.log(result),
      _ => console.log("The file count = 0; that is a problem?")
    );
    this.setCurrentPage();
  }


}
