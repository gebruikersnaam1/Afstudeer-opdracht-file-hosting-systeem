
export interface CreateFolderData{
    folderName: string;
    parentID: number;
}

export type folderView = {
    folderID : number;
    totalItems : number; 
    itemsPerPages : number;
    currentPage : number;
  }