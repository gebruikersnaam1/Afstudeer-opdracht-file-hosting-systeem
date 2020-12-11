import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CloudService } from './cloud.service';
import * as FileSaver  from 'file-saver';
import { map } from 'rxjs/operators';
import { FileId, FileInformation,SharedFileInfo } from '../interfaces/file';
import { Observable } from 'rxjs';
declare const WaitCursor: any;
declare const DefaultCursor: any;

@Injectable({
    providedIn: 'root'
  })
export class FileManager {

    constructor(private router : Router, private cloudService : CloudService ){ }
    
    private downloadPopup(data: Blob, fileAssistent : Observable<FileInformation>){
        try{
          var blob = new Blob([data], {type: data.type});
          fileAssistent.subscribe(
             i => FileSaver.saveAs(blob,(i.fileName+i.extension)),
             _ =>this.router.navigateByUrl("/500")
          );
        }catch{
          this.router.navigateByUrl("/500");
        }
      }
      
      deleteFiles(fileIds : FileId[])  : Observable<boolean>{
        return this.cloudService.removeFilesFromFolder(fileIds).pipe(
          map((res)=> res.status === 204 ? true : false)
        )
      }

      deleteFile(fileId: FileId) : Observable<boolean>{
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
    
      downloadFile(fileId : FileId) :  Observable<Blob>{ 
        return this.cloudService.downloadFile(fileId).pipe(
            map((data : Blob)=> {
                WaitCursor();
                this.downloadPopup(data,this.cloudService.downloadFileAssistent(fileId));
                DefaultCursor();
                return data;
            })
          );
      }

      downloadFiles(fileIds : FileId[]) :  Observable<Blob>{ 
        return this.cloudService.downloadFiles(fileIds).pipe(
            map((data : Blob)=> {
                WaitCursor();
                FileSaver.saveAs(data,"bestanden.zip");
                DefaultCursor();
                return data;
            })
          );
      }

      moveFile(blobId : number, fileId : number) : Observable<boolean>{ 
        return this.cloudService.moveFileToAnotherFolder(blobId,fileId).pipe(
          map(r => r?.status === 200 ? true : false)
        );
      }

      copyFile(blobId : number, folderId : number) : Observable<number | boolean>{
        return this.cloudService.copyFileToAnotherFolder(blobId, folderId).pipe(
          map(file => !!file.fileId && file != null ? Number(file.fileId) : false)
        );
      }

      shareFile(blobId: number) : Observable<boolean| string>{
        return this.cloudService.setFileToShare(blobId).pipe(map(f => f != null && !!f.id != false ? f.id : false))
      }

      getSharedFileInfo(shareFileId: number) : Observable<SharedFileInfo> {
        return this.cloudService.getSharedFileInfo(shareFileId);
      }

      downloadSharedFile(shareFileId : number) : Observable<Blob>{
        return this.cloudService.downloadSharedFile(shareFileId).pipe(
          map((data : Blob)=> {
              WaitCursor();
              this.downloadPopup(data,this.cloudService.downloadSharedFileAssistent(shareFileId));
              DefaultCursor();
              return data;
          })
        );
      }
}
