import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'bytesConverter'
})
export class BytesConverterPipe implements PipeTransform {

  transform(kbSize: number): string {
    let mb = Math.round(kbSize / Math.pow(1024,1));
    let gb = Math.round(kbSize / Math.pow(1024,2));
    let tb = Math.round(kbSize / Math.pow(1024,3));

    if(tb >= 1){
      return tb+"tb";
    }
    if(gb >= 1){
      return gb+"gb";
    }
    if(mb >= 1){
      return mb+"mb";
    }
    return kbSize+"kb";
  }

}
