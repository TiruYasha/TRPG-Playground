import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapManagerComponent } from './map-manager.component';

describe('MapManagerComponent', () => {
  let component: MapManagerComponent;
  let fixture: ComponentFixture<MapManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
