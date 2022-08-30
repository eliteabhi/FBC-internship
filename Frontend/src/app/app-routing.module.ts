import { QuestionareComponent } from './components/questionare/questionare.component';
import { SurveyAnswerComponent } from './components/survey-answer/survey-answer.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SurveyCompletionComponent } from './components/survey-completion/survey-completion.component';

const routes: Routes = [
  {path: "", component: SurveyAnswerComponent,},
  {path: "survey", component: QuestionareComponent,},
  {path: "completion", component: SurveyCompletionComponent,}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
