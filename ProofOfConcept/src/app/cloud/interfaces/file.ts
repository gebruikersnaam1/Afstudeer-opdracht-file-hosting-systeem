export interface fileData {
    fileId: string;
    userId: string;
    fileName: string;
    date: string;
    pathFile: string;
    description: string;
    fileSize: string;
}

export interface FileId extends Pick<fileData, 'fileId'> {}

export interface FileInformation{
    fileName: string;
    extension : string;
}

export interface FileSharingData{
    id: string,
    blobId: number,
    activeUntil : Date
}

export interface SharedFileInfo{
    shareId: number,
    fileName: string,
    availableUntil : Date
}
