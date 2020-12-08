import { Injectable } from '@angular/core';
import { fileData, FileId,FileInformation } from '../interfaces/file';
import { CreateFolderData,ExplorerData, FolderStructure, Folder, ChangeFolder } from '../interfaces/folder';

import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../auth/shared/auth.service';


@Injectable({
  providedIn: 'root'
})
export class CloudService {
  url: string = "https://localhost:44368/api/";

  constructor(private client: HttpClient, private authService : AuthService) { }


  private fileIdsToParams(fileIds: FileId[]){
    let z = "?";
    fileIds.forEach(element => {
      z += ("blobIds=" + element + "&")
    });
    return z.slice(0,-1);
  }
  /*************************************
    @files get files (with folders) based on search or current 'page'
  *************************************/
  getFiles(itemsPerPages : number, currentPage : number){
    let getUrl = this.url+"blobfiles/?itemsPerPage=" + itemsPerPages + "&currentPage=" + currentPage;
    return this.client.get<fileData[]>(getUrl);
  }

  getFile(file : FileId){
    return this.client.get<fileData>((this.url+"blobfiles/file/?id="+file.fileId));
  }

  searchOnFileName(searchTerm){
    return this.client.get<fileData[]>((this.url+"blobfiles/search/" + searchTerm.toString()));
  }

  getFileCount(){
    return this.client.get<number>((this.url+"blobfiles/count"));
  }

  /*************************************
    @folderManagement create and delete file
  *************************************/
  createFolder(data : CreateFolderData){
    return this.client.post<Folder>((this.url+"folders/create"),data);
  }

  validateFolderID(folderID : number){
    return this.client.get((this.url+"folders/validateid/"+folderID));
  }

  searchInFolders(searchTerm){
    return this.client.get<ExplorerData[]>((this.url+"folders/search/" + searchTerm.toString()));
  }

  getFolderContent(folderID: number){
    return this.client.get<ExplorerData[]>((this.url+"folders/getFolderContent/"+folderID));
  }

  getFolderStructure(){
    return this.client.get<FolderStructure>((this.url+"folders/folderStructure"));
  }

  /*************************************
    @folderFileManagement Download, create and delete file with the folder structure
  *************************************/
  uploadFileInAssignedFolder(file : FormData){
    return this.client.post<fileData>((this.url+"folders/upload"), file);
  }

  getFolder(folderId : number){
    return this.client.get<Folder>((this.url+"folders/getFolder/"+folderId));
  }

  changeFolderName(data : ChangeFolder){
    return this.client.put((this.url+"folders/changeFolder/"),data);
  }

  removeFolder(folderId: number){
    return this.client.delete((this.url+"folders/deleteFolder/"+folderId))
  }

  deleteFileFromFolder(file: FileId){
    return this.client.delete((this.url+"folders/removeBlobFromFolders/"+file.fileId), { responseType: 'text', observe: 'response' });
  }

  removeFilesFromFolder(fileIds: FileId[]){
    const params = this.fileIdsToParams(fileIds);
    return this.client.delete((this.url+"folders/blobsRemoveFromFolder/"+params),{ responseType: 'text', observe: 'response' });
  }

  syncFiles(){
    return this.client.get((this.url+"folders/synchronicFiles"));
  }

  getFolderOfFile(blobId: number){
    //the id will be validate 
    return this.client.get<Folder>((this.url+"folders/findFolder/"+blobId));
  }

  moveFileToAnotherFolder(blobId: number, folderId : number){
    //the id will be validate 
    return this.client.put((this.url+"folders/moveBlob?blobId=" + blobId +"&folderId="+folderId), {},{ responseType: 'text', observe: 'response' });
  }

  copyFileToAnotherFolder(blobId: number, folderId : number){
    const body = {
      blobId: blobId,
      folderId: folderId
    };
    //the id will be validate 
    return this.client.post<fileData>((this.url+"folders/copyFolder"), body);
  }


  
  /*************************************
    @blobManagement Download, create and delete file
  *************************************/
  downloadFileAssistent(file: FileId){
    return this.client.get<FileInformation>(((this.url+"blobfiles/download/assistent/?id="+file.fileId)))
  }

  downloadFile(file: FileId){
    return this.client.get(((this.url+"blobfiles/download/?id="+file.fileId)),{
      responseType: 'blob'
    })
  }

  downloadFiles(fileIds: FileId[]){
    const params = this.fileIdsToParams(fileIds);
    return this.client.get(((this.url+"blobfiles/downloadFiles/"+params)),{
      responseType: 'blob'
    }
    )
  }

  updateFile(file: fileData){
    //the id will be validate 
    return this.client.put((this.url+"blobfiles/update"),file);
  }

  uploadFile(file : FormData){
    return this.client.post<fileData>((this.url+"blobfiles/upload"), file);
  }
}
