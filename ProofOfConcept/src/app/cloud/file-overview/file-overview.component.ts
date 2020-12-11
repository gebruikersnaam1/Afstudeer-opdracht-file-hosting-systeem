import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CloudService } from "../shared/cloud.service";
import { Fun } from '../../globals';

import { folderView, ExplorerData, Folder } from '../interfaces/folder';

@Component({
  selector: 'cloud-overview',
  templateUrl: './file-overview.component.html',
  styleUrls: ['./file-overview.component.scss']
})
export class FileOverviewComponent implements OnInit {
  folder : Folder;
  rawData : ExplorerData[]; //data that isn't filterd
  rows : ExplorerData[];
  filesLoaded: Promise<boolean>;
  errorExist = false;

  dataFilter : Fun<ExplorerData[],ExplorerData[]> = Fun(x => x);
  
  folderData : folderView = {
    currentfolderID : 1, //root ID
    totalItems : 0,
    itemsPerPages : 10
  }
  

  constructor(private cloudService: CloudService, private activeRoute : ActivatedRoute, private router : Router) { }

  ngOnInit(): void {
    this.setCurrentPage();
  }

  loadFiles(data : ExplorerData[]){
    this.rawData = data; 
    this.setRows(); 
    this.filesLoaded = Promise.resolve(true);
  }

  searchForFile(searchTerm : string){
    if(searchTerm == "" || searchTerm === undefined){
      this.setCurrentPage();
      return;
    }
    this.cloudService.searchInFolders(searchTerm).subscribe(
      files => this.loadFiles(files),
      _ => this.loadFiles([])
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

  goToFile(fileId:number){
    this.router.navigateByUrl(("/cloud/file/"+fileId));
  }

  changeFolder(folderId : number){
    this.folderData.currentfolderID = folderId > 0 ? folderId : 1;
    this.setFiles();
  }

  setCurrentPage(){
    this.activeRoute.params.subscribe( 
      (value) => {
        if(value?.folderID == undefined || Number(value?.folderID) == NaN){
          this.folderData.currentfolderID = 1;
        }else{
          this.folderData.currentfolderID = value.folderID;
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
      files => this.loadFiles(files),
      _ => { this.errorExist = true; this.filesLoaded = Promise.resolve(false);}
    );
  }

  syncFiles() {
    this.cloudService.syncFiles().subscribe(_ => this.changeFolder(1));
  }

  setRows(){
    this.rows = this.dataFilter.f(this.rawData);
  }

  setFilter(filter){
    this.dataFilter = filter;
    this.setRows();
  }
}
