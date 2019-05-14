import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'linebreak'
})
export class LinebreakPipe implements PipeTransform {

  transform(value: any, args?: any): any {0

    if(value === undefined)
      return value;

    return value.replace(/\n/g, "<br />");
  }

}
