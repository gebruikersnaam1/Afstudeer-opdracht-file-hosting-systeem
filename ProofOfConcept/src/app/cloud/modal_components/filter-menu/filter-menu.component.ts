import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ExplorerData } from '../../interfaces/folder';
import { Fun } from '../../../globals';

@Component({
  selector: 'cloud-filter-menu',
  templateUrl: './filter-menu.component.html',
  styleUrls: ['./filter-menu.component.scss']
})
export class FilterMenuComponent implements OnInit {
  @Output() filter = new EventEmitter<Fun<ExplorerData[],ExplorerData[]>>();

  startDate : Fun<ExplorerData[],ExplorerData[]>;
  endDate : Fun<ExplorerData[],ExplorerData[]> = Fun(x=>x);
  keywordsFilter : Fun<ExplorerData[],ExplorerData[]> = Fun(x=>x);
  value = false;

  constructor() { 
  }

  ngOnInit(): void {}

  resetFilter(){
    this.startDate =Fun(x=>x);
    this.endDate =Fun(x=>x);
    this.keywordsFilter =Fun(x=>x);
  }

  setFilter(){
    console.log(this.value);
    const f = this.startDate.then(this.endDate).then(this.keywordsFilter);
    this.filter.emit(f);
  }

  createFun(condition: (x:ExplorerData) => boolean) : Fun<ExplorerData[],ExplorerData[]>{
    return Fun(
      f => {
        let newData : ExplorerData[] = [];
        f.forEach(e => {
          if(condition(e)){
            newData.push(e);
          }
        });
        return newData;
      }
    );
  }

  onMinDate(date:Date){
    if(date.toString() == ""){
      this.startDate = Fun(x => x);
    }else{
      this.startDate = this.createFun(
        e => { console.log("min_data"); return e.lastChanged >= date ? true : false }  
      );
    }
    this.setFilter();
  }

  onMaxDate(date:Date){
    if(date.toString() == ""){
      this.endDate = Fun(x => x);
    }else{
      this.endDate = this.createFun(
        e => { console.log("max_data");  return e.lastChanged <= date ? true : false} 
      );
    }
    this.setFilter();
  }

  onKeyWords(keySentence: string){
    if(keySentence == null){
      this.keywordsFilter = Fun(x=>x);
    }else{
      const keywords = keySentence.split(",");
      this.keywordsFilter = this.createFun(
        e => {
          let founded = false;
          if(e.keywords == null) { return false; }
  
          keywords.forEach(z => e.keywords.indexOf(z) > 1 ? founded = true : false)
          return founded;
        } 
      );
    }
    this.setFilter();
  }


  

}
