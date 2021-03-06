export interface CreateFolderData{
    folderName: string;
    parentID: number;
}

export interface folderView {
    currentfolderID : number;
    totalItems : number; 
    itemsPerPages : number;
}

export interface ExplorerData{
    name: string,
    lastChanged : Date,
    id: number,
    keywords: string[]
    size : number,
    type: string,
    isFolder :boolean
}

export interface Folder{
    folderId : number
    folderName: string,
    parentFolder: Folder,
    dateChanged : Date,
    createdDate: Date
}

export interface ChangeFolder{
    folderName : string,
    folderId : number
}

export interface FolderStructure{
    currentBranch: Folder,
    childBranches: FolderStructure[]
}


export interface FolderStructureNode {
    expandable: boolean;
    folderName: string;
    folderId: number;
    level: number;
}