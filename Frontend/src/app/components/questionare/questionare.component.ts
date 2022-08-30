import { DataService } from './../../data-service.service';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as surveys from './Surveys.json';

@Component({
  selector: 'app-questionare',
  templateUrl: './questionare.component.html',
  styleUrls: ['./questionare.component.scss']
})
export class QuestionareComponent implements OnInit {

  public questions: any[] = [];
  public doneMsg: string = "Done"

  private _surveyId: number = 1
  private _answers: {surveyId: number, questionId: number, answer: string, userCreated: string}[] = []
  private _restrictUntilPreviousAnswered: {qId: number, parentQId: number}[] = []

  constructor (
    private _http: HttpClient,
    private _DataService: DataService
  ) { }

  ngOnInit(): void {

    const URL: string = 'https://localhost:7071/api/Questions'

    const parsedJson = JSON.parse(JSON.stringify(surveys))

    let surveyQuestions: any[] = parsedJson[this._surveyId - 1].questions

    console.log('Scaffold: ')
    console.log(surveyQuestions)

    this._getData(URL, surveyQuestions)

  }

  private _getData(URL: string, Scaffold: any[]): void {

    let idString: string = ""

    for (const quest of Scaffold) {

      idString += `ids=${quest.questionId}&`

    }

    idString = idString.substring(0, idString.length - 1);

    console.log(`URL: ${URL}/array?${idString}`)

    this._http.get(`${URL}/array?${idString}`).subscribe(async quests => {

      console.log(quests as any[])

      this.questions = quests as any[]

      for (let i = 0; i < Scaffold.length; i++) {

        if (Scaffold[i].isRestricted.answer) {
          this._restrictUntilPreviousAnswered.push ({
            qId: this.questions[i].questionId,

            parentQId: Scaffold[i].isRestricted.parentQuestionId

          });

        }

      }

    });

  }

  isRestricted(qid: number): boolean { return this._restrictUntilPreviousAnswered.findIndex(q => q.qId == qid) != -1; }

  parentAnswered(qid: number): boolean {

    if (this._answers.toString() === '') return false;

    return this._answers!.find(a =>
      a.questionId == this._restrictUntilPreviousAnswered.find(q =>
        q.qId == qid
        )!.parentQId)?.answer === 'Yes';
  }

  answer(questionId: number, answer: string): void {

    console.log(`QuestionId ${questionId} answered with: ${answer}\n`)

    const newAns = {
      surveyId: this._surveyId,
      questionId: questionId,
      answer: answer,
      userCreated: 'Survey'
    }

    const index = this._answers.findIndex(ans => ans.questionId == questionId);

    if (index == -1) {

      this._answers.push(newAns)

    } else {

      this._answers[index] = newAns

    }

    console.log(this._answers)

  }

  onSubmit(): void {

    console.log('Final Answers: ', this._answers)
    this._DataService.qandas = {'qAndA': this._answers}
    this._DataService.isSubmitted = true
    this._DataService.completeSubmit('isSubmitted')

  }

}
