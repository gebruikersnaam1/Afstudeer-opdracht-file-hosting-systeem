import { Injectable } from '@angular/core';
import { fileData, FileId,FileInformation } from '../interfaces/file';
import { CreateFolderData,FolderResponse, Folder, ChangeFolder } from '../interfaces/folder';

import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../auth/shared/auth.service';


@Injectable({
  providedIn: 'root'
})
export class CloudService {
  url: string = "https://localhost:44368/api/";

  constructor(private client: HttpClient, private authService : AuthService) { }

  /*************************************
    @files get files (with folders) based on search or current 'page'
  *************************************/
  getFiles(itemsPerPages : number, currentPage : number){
    let getUrl = this.url+"blobfiles/?itemsPerPage=" + itemsPerPages + "&currentPage=" + currentPage;
    return this.client.get<fileData[]>(getUrl,{
      headers:{
         authorization: ("Bearer " + this.authService.id_token)
        }
    });
  }

  getFile(file : FileId){
    return this.client.get<fileData>((this.url+"blobfiles/file/?id="+file.fileId), {
      // headers:{
      //   authorization: ("Bearer " + this.authService.id_token)
      //  }
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

  /*************************************
    @folderManagement create and delete file
  *************************************/
  createFolder(data : CreateFolderData){
    return this.client.post<Folder>((this.url+"folders/create"),data, {
        headers:{
          authorization: ("Bearer " + this.authService.id_token)
          }
    });
  }

  validateFolderID(folderID : number){
    return this.client.get((this.url+"folders/validateid/"+folderID),{
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  searchInFolders(searchTerm){
    return this.client.get<FolderResponse[]>((this.url+"folders/search/" + searchTerm.toString()), {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  getFolderContent(folderID: number){
    return this.client.get<FolderResponse[]>((this.url+"folders/getFolderContent/"+folderID),{
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }


  /*************************************
    @folderFileManagement Download, create and delete file with the folder structure
  *************************************/
  uploadFileInAssignedFolder(file : FormData){
    return this.client.post<fileData>((this.url+"folders/upload"), file,
    {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  getFolder(folderId : number){
    return this.client.get<Folder>((this.url+"folders/getFolder/"+folderId), 
    {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  changeFolderName(data : ChangeFolder){
    return this.client.put((this.url+"folders/changeFolder/"),data, 
    {
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    });
  }

  removeFolder(folderId: number){
    return this.client.delete((this.url+"folders/deleteFolder/"+folderId),{
      headers: {
        authorization: ("Bearer " + this.authService.id_token)
      }
    })
  }
  
  /*************************************
    @blobManagement Download, create and delete file
  *************************************/
  downloadFileAssistent(file: FileId){
    return this.client.get<FileInformation>(((this.url+"blobfiles/download/assistent/?id="+file.fileId)),{
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    })
  }

  downloadFile(file: FileId){
    return this.client.get(((this.url+"blobfiles/download/?id="+file.fileId)),{
      responseType: 'blob' ,
      headers:{
        authorization: ("Bearer " + this.authService.id_token)
       }
    })
  }

  deleteFile(file: FileId){
    return this.client.delete((this.url+"blobfiles/delete/?id="+file.fileId), {
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
