import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ExplorerData } from '../../interfaces/folder';
import { Fun } from '../../../globals';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'cloud-filter-menu',
  templateUrl: './filter-menu.component.html',
  styleUrls: ['./filter-menu.component.scss']
})
export class FilterMenuComponent implements OnInit {
  @Output() filter = new EventEmitter<Fun<ExplorerData[],ExplorerData[]>>();

  startDate : Fun<ExplorerData[],ExplorerData[]> = Fun(x=>x);
  endDate : Fun<ExplorerData[],ExplorerData[]> = Fun(x=>x);
  keywordsFilter : Fun<ExplorerData[],ExplorerData[]> = Fun(x=>x);

  filterGroup = new FormGroup({
    startDate: new FormControl(),
    endDate : new FormControl(),
    searchTerms: new FormControl()
  })

  constructor() { 
  }

  ngOnInit(): void {}

  resetFilter(){
    this.startDate =Fun(x=>x);
    this.endDate =Fun(x=>x);
    this.keywordsFilter =Fun(x=>x);

    this.filterGroup.reset();
    this.setFilter();
  }

  setFilter(){
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

  onMinDate(event:any){
    const date = event.target?.value;
    if(date.toString() == ""){
      this.startDate = Fun(x => x);
    }else{
      this.startDate = this.createFun(
        e => e.lastChanged >= date ? true : false   
      );
    }
    this.setFilter();
  }

  onMaxDate(event: any){
    const date = event.target?.value;
    if(date.toString() == ""){
      this.endDate = Fun(x => x);
    }else{
      this.endDate = this.createFun(
        e => e.lastChanged <= date ? true : false
      );
    }
    this.setFilter();
  }

  onKeyWords(event: any){
    const keySentence = event.target?.value;

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
