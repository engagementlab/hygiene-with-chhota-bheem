import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'prettyUrl'
})
export class PrettyUrlPipe implements PipeTransform {

  transform(value: string): string {
    return value.replace(/(^\w+:|^)\/\//, '').replace('www.', '');
  }

}
