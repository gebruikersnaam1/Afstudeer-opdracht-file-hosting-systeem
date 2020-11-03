import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchbalkComponent } from './searchbalk.component';

describe('SearchbalkComponent', () => {
  let component: SearchbalkComponent;
  let fixture: ComponentFixture<SearchbalkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchbalkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchbalkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
