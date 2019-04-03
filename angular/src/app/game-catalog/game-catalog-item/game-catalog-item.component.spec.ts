import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameCatalogItemComponent } from './game-catalog-item.component';

describe('GameCatalogItemComponent', () => {
  let component: GameCatalogItemComponent;
  let fixture: ComponentFixture<GameCatalogItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameCatalogItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameCatalogItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
