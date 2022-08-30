import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyCompletionComponent } from './survey-completion.component';

describe('SurveyCompletionComponent', () => {
  let component: SurveyCompletionComponent;
  let fixture: ComponentFixture<SurveyCompletionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SurveyCompletionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SurveyCompletionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
