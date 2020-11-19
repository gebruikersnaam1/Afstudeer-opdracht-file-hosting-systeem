import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FolderNavigationComponent } from './folder-navigation.component';

describe('FolderNavigationComponent', () => {
  let component: FolderNavigationComponent;
  let fixture: ComponentFixture<FolderNavigationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FolderNavigationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FolderNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
