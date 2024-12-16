//https://blog.briebug.com/blog/how-do-i-display-html-inside-an-angular-binding

import { Pipe, PipeTransform } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";

import { UtcConverterService } from '../services/UtcConverterService';

//https://nishugoel.medium.com/creating-custom-pipes-in-angular-2b082a5dc74b
@Pipe({
    name: "safeHtml",
    standalone: false
})
export class SafeHtmlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) { }

  transform(value) {
    return this.sanitizer.bypassSecurityTrustHtml(value);
  }
}

@Pipe({
    name: 'safeUrl',
    standalone: false
})
export class SafeUrlPipe implements PipeTransform {
  constructor(private domSanitizer: DomSanitizer) { }
  transform(url) {
    return this.domSanitizer.bypassSecurityTrustResourceUrl(url);
  }
} 


@Pipe({
    name: 'orderNumberFilter',
    pure: false,
    standalone: false
})
//export class OrderNumberFilter implements PipeTransform {
//  //transform(items: OrderOfList[], filter: number): any {
//  //  if (!items || !filter) {
//  //    return items;
//  //  }
//  //  // filter items array, items which match and return true will be
//  //  // kept, false will be filtered out
//  //  return items.filter(item => item.invno = filter);
//  }
//}
@Pipe({
    name: 'YesNo',
    standalone: false
})
export class YesNoPipe implements PipeTransform {
  transform(value: any): any {
    return value ? 'Yes' : 'No';
  }
}
@Pipe({
    name: 'Level',
    standalone: false
})
export class Level implements PipeTransform {
  transform(value: any): any {
   
    switch (value) {
      case 1:
        return "Important"
        break;
      case 2:
        return "Warning"
        break;
      case 3:
        return "Information"
        break;
      default:
        return ""
    }
  }
}

@Pipe({
    name: 'utcToLocalTime',
    standalone: false
})
export class UtcToLocalTimePipe implements PipeTransform {

  constructor(private _dateConverter: UtcConverterService) {
//
  }

  transform(date: string, args?: any): string {
   return this._dateConverter.convertUtcToLocalTime(date, args);
    
  }
}

@Pipe({
    name: 'decimalPlaces',
    standalone: false
})
export class DecimalPlaces implements PipeTransform {
  //differenc between angular and this is this return number and angular returns string
  transform(value: number, places: number): number {
    const factor = 10 ** places;
    return Math.round(value * factor) / factor;
  }
}
@Pipe({
    name: 'productTypeFilter',
    pure: false,
    standalone: false
})
export class ProductTypeFilter implements PipeTransform {
  transform(items: any[], filter: string): any {
    if (!items || !filter) {
      return items;
    }
    // filter items array, items which match and return true will be
    // kept, false will be filtered out
    return items.filter(item => item.itemType ==filter);
  }
}
