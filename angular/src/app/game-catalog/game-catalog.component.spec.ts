import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameCatalogComponent } from './game-catalog.component';

describe('GameCatalogComponent', () => {
  let component: GameCatalogComponent;
  let fixture: ComponentFixture<GameCatalogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameCatalogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameCatalogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
