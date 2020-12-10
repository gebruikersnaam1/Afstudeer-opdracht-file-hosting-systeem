import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {SharedFileInfo} from '../interfaces/file';
import { FileManager} from '../shared/filemanager';

@Component({
  selector: 'app-public-file',
  templateUrl: './public-file.component.html',
  styleUrls: ['./public-file.component.scss']
})
export class PublicFileComponent implements OnInit {
  error = false;
  file : SharedFileInfo = null;

  constructor(public activeRouter : ActivatedRoute, private fileManager : FileManager) { }

  ngOnInit(): void {
    this.activeRouter.params.subscribe((id : {id:number})  => 
       Number(id?.id).toString() != "NaN" ? this.getFileInfo(id.id) : this.error = true
    )
  }

  getFileInfo(id: number){
    this.fileManager.getSharedFileInfo(id).subscribe(
      result => this.file = result,
      _ => this.error = true
      );
  }

  Download(){
    this.fileManager.downloadSharedFile(this.file.shareId).subscribe();
  }

}
