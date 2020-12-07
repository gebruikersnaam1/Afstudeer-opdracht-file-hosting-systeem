import { Component, OnInit } from '@angular/core';
import { fileData,FileId } from '../interfaces/file';
import { CloudService } from '../shared/cloud.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FileManager } from '../shared/filemanager';
import { Folder } from '../interfaces/folder';
import { map } from 'rxjs/operators';

declare const WaitCursor: any;
declare const DefaultCursor: any;


@Component({
  selector: 'cloud-file',
  templateUrl: './show-file.component.html',
  styleUrls: ['./show-file.component.scss']
})
export class ShowFileComponent implements OnInit {
  fileGroup: FormGroup;
  file : fileData;
  folder: Folder;
  fileLoaded: Promise<boolean>;
  fileDeleted = false;
  fileUpdated = false;

  constructor(private cloudService : CloudService, private fileManager: FileManager, private activeRoute : ActivatedRoute, private router : Router) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe( (id: FileId) => {
      this.cloudService.getFile(id).subscribe(
        file => {
              this.file = file;
              this.fileGroup = new FormGroup({
                fileName: new FormControl(this.file.fileName, [
                  Validators.required,
                  Validators.minLength(2),
                  Validators.maxLength(40)
                ]),
                description: new FormControl(this.file.description,[
                  Validators.required,
                  Validators.minLength(2),
                  Validators.maxLength(150)
                ])
              });
              this.setFolder();
              this.fileLoaded = Promise.resolve(true);
            },
        _ => this.router.navigateByUrl("404")
      )
    });
  }

  GetUsername(userID: string){
    return userID;
  }

  setFolder(){
    this.cloudService.getFolderOfFile(Number(this.file.fileId)).subscribe( 
      (f: Folder) => this.folder = f,
      _ => this.router.navigateByUrl("/500"));
  }

  changeFolder(folderId:number){
    this.fileManager.moveFile(Number(this.file.fileId), folderId).subscribe(
        r => r == true ? window.location.reload() : this.router.navigateByUrl("/500") );
  }

  onDownloadClick(){
    this.fileManager.downloadFile(this.file).subscribe(
      data => console.log(data),
      _ => this.router.navigateByUrl("/500")
    );
  }
  

  copyFile(folderId:number){
    this.fileManager.copyFile(Number(this.file.fileId), folderId).subscribe(
      id => this.router.navigateByUrl(("/cloud/file/"+id)),
      _ => this.router.navigateByUrl("/500")
    )
  }

  deleteFile(){
    WaitCursor();
    this.fileManager.deleteFile(this.file).subscribe(
      result => result === true ? this.fileDeleted = true : this.router.navigateByUrl("500")
    );
    DefaultCursor();
  }

  updateFile(){
    const {fileName, description} = this.fileGroup.value;
    const newFile = {...this.file, fileName : fileName, description : description} 
    this.cloudService.updateFile(newFile).subscribe(
      result => this.fileUpdated = true,
      _ => this.router.navigateByUrl("500")
    );
  }

}
