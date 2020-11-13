import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CloudService } from '../shared/cloud.service';
import { CreateFolderData } from '../interfaces/folder';

@Component({
  selector: 'app-createfolder',
  templateUrl: './createfolder.component.html',
  styleUrls: ['./createfolder.component.scss']
})
export class CreatefolderComponent implements OnInit {
  folderGroup = new FormGroup({
    folderName: new FormControl('',[
      Validators.required,
      Validators.minLength(1)
    ])
  })
  parentFolderID : number;

  constructor(private cloudService : CloudService, private router : Router, private activeRoute : ActivatedRoute) { }

  ngOnInit(): void {
    this.activeRoute.params.subscribe((obj)=> this.parentFolderID = Number(obj?.id));
    this.cloudService.validateFolderID(this.parentFolderID).subscribe(
      result => console.log("parent folder was found"),
      _ => this.router.navigateByUrl("500")
    )
  }

  GetFolderData() : CreateFolderData{
    const { folderName } = this.folderGroup.value;
    return {
        folderName: folderName,
        parentID: this.parentFolderID
    }
  }

  onSubmit(){
    this.cloudService.createFolder(this.GetFolderData()).subscribe(
      result => console.log("gelukt!"),
      _ => this.router.navigateByUrl("500")
    )
  }

}
