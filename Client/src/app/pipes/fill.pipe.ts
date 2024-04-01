import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'fill'
})
export class FillPipe implements PipeTransform {
  transform(value: number): Array<number> {
    return new Array<number>(value).fill(1);
  }
}
