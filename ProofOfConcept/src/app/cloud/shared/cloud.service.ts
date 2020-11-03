import { Injectable } from '@angular/core';
import { fileData, FileId,FileInformation } from '../interfaces/file';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../auth/shared/auth.service';


@Injectable({
  providedIn: 'root'
})
export class CloudService {
  url: string = "https://localhost:44368/api/";

  constructor(private client: HttpClient, private authService : AuthService) { }

  getFiles(itemsPerPages : number, currentPage : number){
    let getUrl = this.url+"blobfiles/?itemsPerPage=" + itemsPerPages + "&currentPage=" + currentPage;
    return this.client.get<fileData[]>(getUrl,{
      headers:{
         authorization: ("Bearer " + this.authService.id_token)
        }
    });
  }

  getFile(file : FileId){
    return this.client.get<fileData>((this.url+"blobfiles/file/"+file.fileId), {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  searchOnFileName(searchTerm){
    return this.client.get<fileData[]>((this.url+"blobfiles/search/" + searchTerm.toString()), {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  getFileCount(){
    return this.client.get<number>((this.url+"blobfiles/count"), {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  downloadFileAssistent(file: FileId){
    return this.client.get<FileInformation>(((this.url+"blobfiles/download/assistent/")+file.fileId),{
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    })
  }

  downloadFile(file: FileId){
    return this.client.get(((this.url+"blobfiles/download/")+file.fileId),{
      responseType: 'blob' ,
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    })
  }

  deleteFile(file: FileId){
    return this.client.delete((this.url+"blobfiles/delete/")+file.fileId, {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  updateFile(file: fileData){
    //the id will be validate 
    return this.client.put((this.url+"blobfiles/update"),file, 
    {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  uploadFile(file : FormData){
    return this.client.post<fileData>((this.url+"blobfiles/upload"), file,
    {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }
}
