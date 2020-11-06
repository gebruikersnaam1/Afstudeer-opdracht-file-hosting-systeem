import { Component, OnInit } from '@angular/core';
declare const DefaultCursor: any;

@Component({
  selector: 'app-notfound',
  templateUrl: './notfound.component.html',
  styleUrls: ['./notfound.component.scss']
})
export class NotfoundComponent implements OnInit {

  constructor() { 
    DefaultCursor(); //wait cursor may be loaded in prev. page, hence this may be needed.
  }

  ngOnInit(): void {
  }

}
