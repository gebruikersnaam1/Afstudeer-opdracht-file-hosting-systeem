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
    filename: string,
    shareId: number,
    shareable : Date
}
