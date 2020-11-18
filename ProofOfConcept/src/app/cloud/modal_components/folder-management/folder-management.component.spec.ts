import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolderManagementComponent } from './folder-management.component';

describe('FolderManagementComponent', () => {
  let component: FolderManagementComponent;
  let fixture: ComponentFixture<FolderManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolderManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolderManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
