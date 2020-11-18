import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CloudService } from '../../shared/cloud.service';
import { CreateFolderData, Folder } from '../../interfaces/folder';

declare const CloseModal: any;

@Component({
  selector: 'cloud-createfolder',
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
  @Input() parentFolderID : number;
  @Input() modalName : string;
  @Output() folderCreated = new EventEmitter<number>();

  constructor(private cloudService : CloudService, private router : Router, private activeRoute : ActivatedRoute) { }

  ngOnInit(): void {
    console.log(this.parentFolderID);
  }

  GetFolderData() : CreateFolderData{
    const { folderName } = this.folderGroup.value;
    return {
        folderName: folderName,
        parentID: Number(this.parentFolderID)
    }
  }

  onSubmit(){
    this.cloudService.createFolder(this.GetFolderData()).subscribe(
      (result:Folder ) =>  { CloseModal(this.modalName); this.folderCreated.emit(result.folderId); },
      _ => { CloseModal(this.modalName); this.router.navigateByUrl("500"); }
    )
  }

}
