import { Component, OnInit, Input, Output, EventEmitter,ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatMenuTrigger } from '@angular/material/menu';
import { ExplorerData } from '../../interfaces/folder';
import { FileManager } from '../filemanager';
import { FileId } from '../../interfaces/file';


type cloudItems = "Folder" | "File";
type TypeMouseClicks = "Download" | "Delete" | "DownloadAll" | "DeleteAll";

@Component({
  selector: 'cloud-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.scss']
})
export class FileTableComponent implements OnInit {
  //empty th in html page self
  headers = [ "Naam", "Datum", "Type", "Bestandsgroten"];

  selectedFiles : FileId[] = [];

  @Input() rows: ExplorerData[];
  @Output() onShowFolderEvent = new EventEmitter<number>();
  @Output() onShowFileEvent = new EventEmitter<number>();
  @Input() currentPage : number = 1;


  sorting = false;

  menuTopLeftPosition =  {x: '0', y: '0'}

  // reference to the MatMenuTrigger in the DOM
  @ViewChild(MatMenuTrigger, {static: true}) menuTrigger: MatMenuTrigger;

  constructor(private router : Router, private fileManager : FileManager) { }

  ngOnInit(): void {
  }

  onClick(id:number, folder: cloudItems){
    folder == "Folder" ? this.onShowFolderEvent.emit(id) : this.onShowFileEvent.emit(id);
  }


  sortList(z: any){
    this.sorting == true ? this.rows.sort(z) : this.rows.sort(z).reverse();
    this.sorting = !this.sorting;
  }

  onSortRequest(headerName: string){
    switch(headerName){
      case this.headers[0]:
        this.sortList((a:ExplorerData, b:ExplorerData) => a.name.localeCompare(b.name));
       break;
       case this.headers[1]:
        this.sortList((a:ExplorerData, b:ExplorerData) => <any>new Date(b.lastChanged) - <any>new Date(a.lastChanged));
       break;
       case this.headers[2]:
        this.sortList((a:ExplorerData, b:ExplorerData) =>  a.type.localeCompare(b.type));
       break;
       case this.headers[3]:
        this.sortList((a:ExplorerData, b:ExplorerData) => a.size -b.size);
       break;
    }
  }

  selectFile(fileId: FileId){
    if(this.selectedFiles.includes(fileId)){
      const index : number = this.selectedFiles.indexOf(fileId);
      this.selectedFiles.splice(index,1);
    }else{
      this.selectedFiles.push(fileId);
    }
    console.log(this.selectedFiles);
  }

  onRightClick(event: MouseEvent, item) {
      event.preventDefault();
      this.menuTopLeftPosition.x = event.clientX + 'px';
      this.menuTopLeftPosition.y = event.clientY + 'px';
      this.menuTrigger.menuData = {item: item}
      this.menuTrigger.openMenu();
  }

  onRightMenuClick(id: string, command: TypeMouseClicks){
    if(command === "Download"){
      this.fileManager.downloadFile({fileId: id}).subscribe();
    }
    else if(command === "Delete"){
      this.fileManager.deleteFile({fileId: id}).subscribe(
        result => result === true ?  this.onShowFolderEvent.emit(this.currentPage) : this.router.navigateByUrl("/500")
      );
    }
    else if(command === "DeleteAll" && this.selectedFiles.length > 1){
      this.fileManager.deleteFiles(this.selectedFiles).subscribe(_ =>  this.onShowFolderEvent.emit(this.currentPage));
    }
    else if(command === "DownloadAll" && this.selectedFiles.length > 1){
      this.fileManager.downloadFiles(this.selectedFiles).subscribe(z => console.log(z));
    }
  }

}
