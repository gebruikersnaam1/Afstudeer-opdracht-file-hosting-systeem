import { Data } from '@angular/router';

export interface CreateFolderData{
    folderName: string;
    parentID: number;
}

export interface folderView {
    currentfolderID : number;
    totalItems : number; 
    itemsPerPages : number;
}

export interface FolderResponse{
    name: string,
    lastChanged : Date,
    id: number,
    size : number,
    type: string,
    isFolder :boolean
}

export interface Folder{
    folderId : number
    folderName: string,
    parentFolder: number,
    dateChanged : Date,
    createdDate: Date
}
