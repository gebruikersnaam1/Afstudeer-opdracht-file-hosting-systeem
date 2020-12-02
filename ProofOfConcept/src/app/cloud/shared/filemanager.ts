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
}
