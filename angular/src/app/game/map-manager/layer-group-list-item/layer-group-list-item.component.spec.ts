import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayerGroupListItemComponent } from './layer-group-list-item.component';

describe('LayerGroupListItemComponent', () => {
  let component: LayerGroupListItemComponent;
  let fixture: ComponentFixture<LayerGroupListItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayerGroupListItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayerGroupListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
