
import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'orderbyPipe'
})
export class OrderBy implements PipeTransform {
  transform(array: Array<any>, args?) {
    // Check if array exists, in this case array contains articles and args is an array that has 1 element : !id
    if (array) {
      array.sort((a: any, b: any) => {
        if (Number(a[args]) < Number(b[args])) {
          return -1;
        } else if (Number(a[args]) > Number(b[args])) {
          return 1;
        } else {
          return 0;
        }
      });
      return array;
    }
    //
  }
}