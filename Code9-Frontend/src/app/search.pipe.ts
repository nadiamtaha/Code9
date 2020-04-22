import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'search'
})
export class SearchPipe implements PipeTransform {

  transform(list:any, searchTxt:any): any {
    if(searchTxt==undefined)
      return list;
    
  return  list.filter(function(list){
       return   list.Name.toLowerCase().includes(searchTxt.toLowerCase())
    })  
  }

}
