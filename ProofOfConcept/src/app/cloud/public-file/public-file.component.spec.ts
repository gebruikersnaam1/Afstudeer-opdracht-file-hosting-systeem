import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PublicFileComponent } from './public-file.component';

describe('PublicFileComponent', () => {
  let component: PublicFileComponent;
  let fixture: ComponentFixture<PublicFileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PublicFileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PublicFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
