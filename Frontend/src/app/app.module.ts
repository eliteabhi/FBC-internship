import { HttpClientModule } from '@angular/common/http'
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { SurveyAnswerComponent } from './components/survey-answer/survey-answer.component';
import { SurveyCompletionComponent } from './components/survey-completion/survey-completion.component';
import { QuestionareComponent } from './components/questionare/questionare.component';
import { ApiInteraceComponent } from './components/api-interace/api-interace.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    SurveyAnswerComponent,
    SurveyCompletionComponent,
    QuestionareComponent,
    ApiInteraceComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
