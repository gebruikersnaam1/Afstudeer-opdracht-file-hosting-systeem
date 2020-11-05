import { Component, OnInit } from '@angular/core';
import { fileData,FileId } from '../interfaces/file';
import { CloudService } from '../shared/cloud.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import * as FileSaver  from 'file-saver';
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
  fileLoaded: Promise<boolean>;
  fileDeleted = false;
  fileUpdated = false;

  constructor(private cloudService : CloudService, private activeRoute : ActivatedRoute, private router : Router) { }

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

              this.fileLoaded = Promise.resolve(true);
            },
        _ => this.router.navigateByUrl("404")
      )
    });
  }

  GetUsername(userID: string){
    return userID;
  }


  downloadPopup(data: any){
    DefaultCursor();
    try{
      var blob = new Blob([data], {type: data.type});
      this.cloudService.downloadFileAssistent(this.file).subscribe(
         i => FileSaver.saveAs(blob,(i.fileName+i.extension)),
         _ =>this.router.navigateByUrl("/500")
      );
    }catch{
      this.router.navigateByUrl("/500");
    }
  }

  downloadFile(){ 
    WaitCursor();
    this.cloudService.downloadFile(this.file).subscribe(
      data => this.downloadPopup(data),
      _ => this.router.navigateByUrl("/500")
    );
  }

  deleteFile(){
    WaitCursor();
    this.cloudService.deleteFile(this.file).subscribe(
      result => this.fileDeleted = true,
      _ => this.router.navigateByUrl("500")
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
