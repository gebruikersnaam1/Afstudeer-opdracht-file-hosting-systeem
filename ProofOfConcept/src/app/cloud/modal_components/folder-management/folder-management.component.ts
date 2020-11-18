import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Folder, ChangeFolder, folderView } from '../../interfaces/folder';
import { CloudService } from '../../shared/cloud.service';
declare const CloseModal: any;


@Component({
  selector: 'cloud-folder-management',
  templateUrl: './folder-management.component.html',
  styleUrls: ['./folder-management.component.scss']
})
export class FolderManagementComponent implements OnInit {
  @Input() folder : Folder;
  @Input() modalName : string;
  @Output() folderRemoved = new EventEmitter<boolean>(false);

  folderUrl: string;
  showSuccesMessage: boolean;

  folderGroup : FormGroup;

  constructor(private cloudService : CloudService, private router : Router) { }

  ngOnInit(): void {
  }

  ngOnChanges(){
    this.folderGroup = new FormGroup({
      folderName : new FormControl(this.folder.folderName,[
          Validators.required,
          Validators.minLength(1),
          Validators.maxLength(25)
      ])
    })
  }

  createChangeFolderObject() : ChangeFolder{
    const { folderName } = this.folderGroup.value;
    return {
      folderName: folderName,
      folderId: this.folder.folderId
    }
  }

  deleteFolder(){
    //
    this.cloudService.removeFolder(this.folder.folderId).subscribe(
      result => {this.folderRemoved.emit(1); CloseModal(this.modalName); },
      _ => { CloseModal(this.modalName); this.router.navigateByUrl("500"); } 
    );
  }

  onSubmit(){
    if(this.folderGroup.invalid){
      return;
    }
    this.cloudService.changeFolderName(this.createChangeFolderObject()).subscribe(
      result => this.showSuccesMessage = true,
      _ =>  { CloseModal(this.modalName); this.router.navigateByUrl("500"); }
    );
  }


  GetFolderRoot(folder : Folder) : string{
    if(folder.parentFolder == null){
      return "root";
    }else{
      return this.GetFolderRoot(folder.parentFolder) + "/" + folder.folderName;
    }
  }

}
