import { Component, OnInit, Input, Output, EventEmitter,ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatMenuTrigger } from '@angular/material/menu';
import { FolderResponse } from '../../interfaces/folder';
import { FileManager } from '../filemanager';

type cloudItems = "Folder" | "File";

@Component({
  selector: 'cloud-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.scss']
})
export class FileTableComponent implements OnInit {
  headers = [ "Naam", "Datum", "Type", "Bestandsgroten"];

  @Input() rows: FolderResponse[];
  @Output() onShowFolderEvent = new EventEmitter<number>();
  @Output() refreshPage = new EventEmitter<boolean>();


  sorting = false;

  menuTopLeftPosition =  {x: '0', y: '0'}

  // reference to the MatMenuTrigger in the DOM
  @ViewChild(MatMenuTrigger, {static: true}) menuTrigger: MatMenuTrigger;

  constructor(private router : Router, private fileManager : FileManager) { }

  ngOnInit(): void {
  }

  onClick(id:number, folder: cloudItems){
    folder == "Folder" ? this.onShowFolderEvent.emit(id) : this.router.navigateByUrl("cloud/file/"+id);
  }


  sortList(z: any){
    this.sorting == true ? this.rows.sort(z) : this.rows.sort(z).reverse();
    this.sorting = !this.sorting;
  }

  onSortRequest(headerName: string){
    switch(headerName){
      case this.headers[0]:
        this.sortList((a:FolderResponse, b:FolderResponse) => a.name.localeCompare(b.name));
       break;
       case this.headers[1]:
        this.sortList((a:FolderResponse, b:FolderResponse) => <any>new Date(b.lastChanged) - <any>new Date(a.lastChanged));
       break;
       case this.headers[2]:
        this.sortList((a:FolderResponse, b:FolderResponse) =>  a.type.localeCompare(b.type));
       break;
       case this.headers[3]:
        this.sortList((a:FolderResponse, b:FolderResponse) => a.size -b.size);
       break;
    }
  }


  onRightClick(event: MouseEvent, item) {
      event.preventDefault();
      this.menuTopLeftPosition.x = event.clientX + 'px';
      this.menuTopLeftPosition.y = event.clientY + 'px';
      this.menuTrigger.menuData = {item: item}
      this.menuTrigger.openMenu();
  }

  onRightMenuClick(id: string, command: "Download" | "Delete"){
    if(command === "Download"){
      this.fileManager.downloadFile({fileId: id}).subscribe();
    }else{
      this.fileManager.deleteFile({fileId: id}).subscribe(
        result => result === true ?  this.refreshPage.emit(true) : this.router.navigateByUrl("500")
      );
    }
  }

}
