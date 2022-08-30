import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionareComponent } from './questionare.component';

describe('QuestionareComponent', () => {
  let component: QuestionareComponent;
  let fixture: ComponentFixture<QuestionareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuestionareComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuestionareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
