import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderSearchBar } from './header-search-bar';

describe('HeaderSearchBar', () => {
  let component: HeaderSearchBar;
  let fixture: ComponentFixture<HeaderSearchBar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderSearchBar],
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderSearchBar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
