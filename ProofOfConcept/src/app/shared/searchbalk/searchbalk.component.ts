import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';

@Component({
  selector: 'app-searchbalk',
  templateUrl: './searchbalk.component.html',
  styleUrls: ['./searchbalk.component.scss']
})
export class SearchbalkComponent implements OnInit {
  @Input() placeholder = "Vul het zoekterm in!";
  @Input() label : string;
  @Output() SearchValueChange = new EventEmitter<string>();

  constructor() {  }

  ngOnInit(): void {
  }

  searchValue(event : string){
    this.SearchValueChange.emit(event);
  }

}
