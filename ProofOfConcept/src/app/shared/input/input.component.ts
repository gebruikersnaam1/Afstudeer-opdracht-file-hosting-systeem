import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss']
})

export class InputComponent implements OnInit {
  @Input() controller : FormControl;
  @Input() label : string = "";
  @Input() inputType : string = "text";
  @Input() formHelper : string;
  @Output() changeInput = new EventEmitter<any>();

  constructor() { }

  ngOnInit(): void {
  }

  Errors(){
    const { touched, dirty, errors } = this.controller;
    return touched && dirty && errors;
  }

  updateEmitter(event){
    this.changeInput.emit(event);
  }
}
