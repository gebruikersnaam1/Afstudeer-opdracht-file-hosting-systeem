import { Component, OnInit, Input, Output, EventEmitter,ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatMenuTrigger } from '@angular/material/menu';
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

  menuTopLeftPosition =  {x: '0', y: '0'}

  // reference to the MatMenuTrigger in the DOM
  @ViewChild(MatMenuTrigger, {static: true}) menuTrigger: MatMenuTrigger;

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


  onRightClick(event: MouseEvent, item) {
      event.preventDefault();
      // we record the mouse position in our object
      this.menuTopLeftPosition.x = event.clientX + 'px';
      this.menuTopLeftPosition.y = event.clientY + 'px';

      this.menuTrigger.menuData = {item: item}

      this.menuTrigger.openMenu();
  }
}
