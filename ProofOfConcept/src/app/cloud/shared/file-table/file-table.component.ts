import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { fileData, FileId } from '../../interfaces/file';
import { Router } from '@angular/router';

@Component({
  selector: 'cloud-table',
  templateUrl: './file-table.component.html',
  styleUrls: ['./file-table.component.scss']
})
export class FileTableComponent implements OnInit {
  headers = ["Bestandsnaam", "Upload Datum", "Uploader", "Bestandsgroten"];
  @Input() rows: fileData[];
  @Output() click = new EventEmitter<string>();

  constructor(private router : Router) { }

  ngOnInit(): void {
  }

  ShowFile(id : FileId){
    this.router.navigateByUrl("/cloud/file/"+id);
  }

}
