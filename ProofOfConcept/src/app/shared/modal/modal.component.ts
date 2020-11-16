import { Component, Input, OnInit } from '@angular/core';
import { Router, NavigationEnd  } from '@angular/router';
declare const CloseModal: any;


@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {
  @Input() buttonClass = "btn btn-custom mb-2";
  @Input() spanClass;
  @Input() buttonText = "";
  @Input() modalID = "myModel"; //important, as so you can have mulitple modals 


  constructor(private router: Router) { }

  ngOnInit(): void {
    this.router.events.subscribe(event => {

      if (event instanceof NavigationEnd) { 
        close();
      }

    });
  }

  close(){
    CloseModal(this.modalID);
  }

}
