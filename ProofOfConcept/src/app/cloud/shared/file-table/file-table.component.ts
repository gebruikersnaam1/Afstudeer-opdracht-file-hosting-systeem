import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { FolderResponse } from '../../interfaces/folder';

@Component({
  selector: 'cloud-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.scss']
})
export class FileTableComponent implements OnInit {
  headers = ["Naam", "Datum", "Type", "Bestandsgroten"];
  @Input() rows: FolderResponse[];
  @Output() onShowFolderEvent = new EventEmitter<number>();

  constructor(private router : Router) { }

  ngOnInit(): void {
  }

  ShowFolder(id:number){
    this.onShowFolderEvent.emit(id);
  }
  
  ShowFile(id : number){
    this.router.navigateByUrl("cloud/file/"+id);
  }

  sortData(headerName: string){
    console.log(headerName);
  }

}
