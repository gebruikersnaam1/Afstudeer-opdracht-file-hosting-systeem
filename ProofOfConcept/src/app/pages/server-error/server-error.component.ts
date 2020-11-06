import { Component, OnInit } from '@angular/core';
declare const DefaultCursor: any;

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {

  constructor() { 
    DefaultCursor(); //wait cursor may be loaded in prev. page, hence this may be needed.
  }

  ngOnInit(): void {
  }

}
