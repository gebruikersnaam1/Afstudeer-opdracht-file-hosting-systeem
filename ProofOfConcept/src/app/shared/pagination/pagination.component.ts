import { Component, OnInit, Input, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventEmitter } from '@angular/core';

interface PaginationNeeded {
  startButton: boolean;
  prevButton: boolean;
  nextButton: boolean;
  lastButton: boolean;
}

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})

export class PaginationComponent implements OnInit {
  @Input() totalItems : number = 0;
  @Input() itemsPerPages : number = 15;
  @Input() url : string = "/404";
  @Output() updatePage  = new EventEmitter<number>();
  
  @Input() currentPage = 0;
  totalPages = 0;
  paginationValues : PaginationNeeded;

  constructor() { }

  ngOnInit(): void {
    this.setPaginationValues();
  }

  getNewPageNumbers(command: string) : number {
    switch(command){
      case 'start':
        return 0;
      case 'prev':
        return ((this.currentPage) - 1) < 0 ? 0 : ((this.currentPage) - 1);
      case 'next':
        return ((this.currentPage) + 1) > this.totalPages ? this.totalPages : ((this.currentPage) + 1);
      case 'last':
        return this.totalPages;
    }
  }

  changePage(command : string){
    this.currentPage = this.getNewPageNumbers(command);
    this.setPaginationValues();
    this.updatePage.emit(this.currentPage);
  }


  resetPaginationValues(){
    this.paginationValues = {
      startButton: false,
      prevButton: false,
      nextButton: false,
      lastButton: false,
    }
  }

  setPaginationValues(){
    this.resetPaginationValues();
    
    this.totalPages = Math.floor((this.totalItems / this.itemsPerPages));
    if( this.totalPages >  2 && this.currentPage > 1){
      this.paginationValues.startButton = true;
    }
    if(this.currentPage > 0){
      this.paginationValues.prevButton = true;
    }
    if(this.currentPage <  this.totalPages){
      this.paginationValues.nextButton = true;
    }
    if(this.currentPage < ( this.totalPages-1)){
      this.paginationValues.lastButton = true;
    }
  }
}
