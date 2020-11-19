import { Component, OnInit } from '@angular/core';
import { CloudService } from '../../shared/cloud.service';
import { FolderStructure } from '../../interfaces/folder';

@Component({
  selector: 'cloud-folder-navigation',
  templateUrl: './folder-navigation.component.html',
  styleUrls: ['./folder-navigation.component.scss']
})
export class FolderNavigationComponent implements OnInit {
  folders : FolderStructure;

  constructor(private cloudService : CloudService) { }

  ngOnInit(): void {
    this.cloudService.getFolderStructure().subscribe((f: FolderStructure) => this.folders = f);
  }

}
