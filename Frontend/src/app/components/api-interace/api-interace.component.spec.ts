import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApiInteraceComponent } from './api-interace.component';

describe('ApiInteraceComponent', () => {
  let component: ApiInteraceComponent;
  let fixture: ComponentFixture<ApiInteraceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ApiInteraceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApiInteraceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
