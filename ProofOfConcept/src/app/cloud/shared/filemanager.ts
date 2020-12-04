import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CloudService } from './cloud.service';
import * as FileSaver  from 'file-saver';
import { map } from 'rxjs/operators';
import { FileId } from '../interfaces/file';

declare const WaitCursor: any;
declare const DefaultCursor: any;

@Injectable({
    providedIn: 'root'
  })
export class FileManager {

    constructor(private router : Router, private cloudService : CloudService ){ }
    
    private downloadPopup(data: Blob,fileId){
        try{
          var blob = new Blob([data], {type: data.type});
          this.cloudService.downloadFileAssistent(fileId).subscribe(
             i => FileSaver.saveAs(blob,(i.fileName+i.extension)),
             _ =>this.router.navigateByUrl("/500")
          );
        }catch{
          this.router.navigateByUrl("/500");
        }
      }

   

      deleteFiles(fileIds : FileId[]){
        return this.cloudService.removeFilesFromFolder(fileIds).pipe(
          map((res)=> res.status === 204 ? true : false)
        )
      }

      deleteFile(fileId: FileId){
        return this.cloudService.deleteFileFromFolder(fileId).pipe(
          map((res)  => {
            if(res.status === 204){
              return true;
            }else{
              return false;
            }
          })
        );
      }
    
      downloadFile(fileId : FileId){ 
        return this.cloudService.downloadFile(fileId).pipe(
            map((data : Blob)=> {
                WaitCursor();
                this.downloadPopup(data,fileId);
                DefaultCursor();
                return data;
            })
          );
      }

      downloadFiles(fileIds : FileId[]){ 
        return this.cloudService.downloadFiles(fileIds).pipe(
            map((data : Blob)=> {
                WaitCursor();
                FileSaver.saveAs(data,"test.zip");
                DefaultCursor();
                return data;
            })
          );
      }

      moveFile(blobId : number, fileId : number){ 
        return this.cloudService.moveFileToAnotherFolder(blobId,fileId).pipe(
          map(r => r?.status === 200 ? true : false)
        );
      }
}
