import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalNodeComponent } from './journal-node.component';

describe('JournalNodeComponent', () => {
  let component: JournalNodeComponent;
  let fixture: ComponentFixture<JournalNodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalNodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalNodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
