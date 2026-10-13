import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProductDetail } from './view-product-detail';

describe('ViewProductDetail', () => {
  let component: ViewProductDetail;
  let fixture: ComponentFixture<ViewProductDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewProductDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(ViewProductDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
