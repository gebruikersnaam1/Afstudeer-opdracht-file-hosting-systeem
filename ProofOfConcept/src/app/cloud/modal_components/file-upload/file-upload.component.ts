import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CloudService } from '../../shared/cloud.service';
import { AuthService } from '../../../auth/shared/auth.service';

import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
declare const WaitCursor: any;
declare const DefaultCursor: any;
declare const CloseModal: any;


@Component({
  selector: 'cloud-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {
  @Input() folderID : number;
  @Input() modalName: string;
  @Output() fileCreated = new EventEmitter<number>();

  file: File;
  userId : string;

  

  fileGroup= new FormGroup({
    fileName: new FormControl('', Validators.required),
    description: new FormControl('',[
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
    ])
  })

  constructor(private cloudService : CloudService, private router : Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.setUserId();
  }

  fileSelected(event){
    this.file = event.target.files[0];
  }

  

  createFileFormat() : FormData{
    const { description } = this.fileGroup.value;

    const formdata = new FormData();
    formdata.append("file", this.file);
    formdata.append("userId", this.userId);
    formdata.append("folderID",this.folderID.toString()); 
    formdata.append("description", description);
    return formdata;
  }

  processDone(){
    DefaultCursor();
    CloseModal(this.modalName);
  }
  setUserId(){
    this.authService.getActiveUserId().then(
      i => this.userId = i
    )
  }

  uploadFile(){
    if(this.fileGroup.invalid || this.file === null){
      return;
    }
    WaitCursor();
    this.cloudService.uploadFileInAssignedFolder(this.createFileFormat()).subscribe(
      result => { this.processDone(); this.router.navigateByUrl(("cloud/file/"+result.fileId)) },
      _ => {  this.processDone(); this.router.navigateByUrl("500") }
    );
  }

}
