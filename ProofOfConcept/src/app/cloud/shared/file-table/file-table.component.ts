import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { FolderResponse } from '../../interfaces/folder';

@Component({
  selector: 'cloud-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.scss']
})
export class FileTableComponent implements OnInit {
  headers = ["", "Naam", "Datum", "Type", "Bestandsgroten"];
  @Input() rows: FolderResponse[];
  @Output() click = new EventEmitter<string>();

  constructor(private router : Router) { }

  ngOnInit(): void {
  }

  ShowFolder(id:number){
    this.router.navigateByUrl("/cloud/explorer/"+id);
  }
  
  ShowFile(id : number){
    this.router.navigateByUrl("/cloud/file/"+id);
  }

}
