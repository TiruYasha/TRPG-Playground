import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateHandoutDialogComponent } from './create-handout-dialog.component';

describe('CreateHandoutDialogComponent', () => {
  let component: CreateHandoutDialogComponent;
  let fixture: ComponentFixture<CreateHandoutDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateHandoutDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateHandoutDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
