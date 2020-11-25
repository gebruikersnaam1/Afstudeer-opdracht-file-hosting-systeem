import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { FolderResponse } from '../../interfaces/folder';


@Component({
  selector: 'cloud-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.scss']
})
export class FileTableComponent implements OnInit {
  headers = [ "Naam", "Datum", "Type", "Bestandsgroten"];

  @Input() rows: FolderResponse[];
  @Output() onShowFolderEvent = new EventEmitter<number>();

  sorting = false;

  constructor(private router : Router) { }

  ngOnInit(): void {
  }

  ShowFolder(id:number){
    this.onShowFolderEvent.emit(id);
  }
  
  ShowFile(id : number){
    this.router.navigateByUrl("cloud/file/"+id);
  }

  sortList(z: any){
    this.sorting == true ? this.rows.sort(z) : this.rows.sort(z).reverse();
    this.sorting = !this.sorting;
  }

  onSortRequest(headerName: string){
    if(headerName === this.headers[0]){
      this.sortList((a:FolderResponse, b:FolderResponse) => a.name.localeCompare(b.name));
    }
   
    if(headerName === this.headers[1]){
      this.sortList((a:FolderResponse, b:FolderResponse) => <any>new Date(b.lastChanged) - <any>new Date(a.lastChanged));
    }
    if(headerName === this.headers[2]){
      this.sortList((a:FolderResponse, b:FolderResponse) =>  a.type.localeCompare(b.type));
    }
    if(headerName === this.headers[3]){
      this.sortList((a:FolderResponse, b:FolderResponse) => a.size -b.size);
    }
    
  }

}
