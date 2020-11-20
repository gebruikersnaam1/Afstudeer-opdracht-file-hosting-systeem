import { Component, OnInit } from '@angular/core';
import { CloudService } from '../../shared/cloud.service';
import { FolderStructure, FolderStructureNode } from '../../interfaces/folder';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';


@Component({
  selector: 'cloud-folder-navigation',
  templateUrl: './folder-navigation.component.html',
  styleUrls: ['./folder-navigation.component.scss']
})
export class FolderNavigationComponent implements OnInit {
  folders : FolderStructure;

  
  constructor(private cloudService : CloudService) { 
  }

  ngOnInit(): void {
    this.cloudService.getFolderStructure().subscribe((f: FolderStructure) => this.dataSource.data = f.childBranches);
  }

  private _transformer = (node: FolderStructure, level: number) => {
    return {
      expandable: !!node.childBranches && node.childBranches.length > 0,
      folderName: node.currentBranch.folderName,
      folderId: node.currentBranch.folderId,
      level: level,
    };
  }

  treeControl = new FlatTreeControl<FolderStructureNode>(
    node => node.level, node => node.expandable);

  treeFlattener = new MatTreeFlattener(
    this._transformer, node => node.level, node => node.expandable, node => node.childBranches);

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  hasChild = (_: number, node: FolderStructureNode) => node.expandable;

}
