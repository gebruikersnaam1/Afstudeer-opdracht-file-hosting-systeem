import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fileTypeIcon'
})
export class FileTypeIconPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    switch(value){
      case 'Folder':
        return "fas fa-folder";
      case '.pdf':
      case '.doc':
      case '.docx':
        return "fas fa-file-alt";
      case '.png':
      case '.jpg':
      case '.svg':
      case '.jpeg':
        return "fas fa-file-image";
      case '.zip':
        return 'fas fa-file-archive';
      default:
        return "fas fa-file";

    }
  }

}
