import { DataService } from './../../data-service.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-survey-answer',
  templateUrl: './survey-answer.component.html',
  styleUrls: ['./survey-answer.component.scss']
})
export class SurveyAnswerComponent implements OnInit {

  infoJson;

  infoForm: FormGroup;
  surveyAnswer: string;
  currentRoute: string;

  constructor (
    private fb: FormBuilder,
    private DataService: DataService
  )
  {

    this.infoForm = this.fb.group({
      juvenileFirstName: '',
      juvenileLastName: '',
      surveyAnswer: 'No',
      surveyId: 1,
      userCreated: 'Survey',
      gaurdians: this.fb.array([]),
      interviewers: this.fb.array([]),
      additional: this.fb.array([])
    });

    this.infoJson = {
      juvenileFirstName: '',
      juvenileLastName:'',
      gaurdians: [{
        gaurdianFirstName: '',
        gaurdianLastName: '',
        gaurdianEmail: '',
        gaurdianPhone: 0,
        UserCreated: 'Survey'
      }],
      interviewers: [{
        interviewerFirstName: '',
        interviewerLastName: '',
        interviewerEmail: '',
        interviewerPhone: 0,
        UserCreated: 'Survey'
      }],
      additional: [{
        additionalFirstName: '',
        additionalLastName: '',
        additionalEmail: '',
        additionalPhone: 0,
        UserCreated: 'Survey'
      }]
    };

    this.surveyAnswer = 'No'
    this.currentRoute = ""

  }

  ngOnInit(): void {

    this.addMember('gaurdians')
    this.addMember('interviewers')

  }

  private _newMember(member: string): FormGroup {

    switch (member) {

      case 'gaurdians':
        return this.fb.group({
          gaurdianFirstName: '',
          gaurdianLastName: '',
          gaurdianEmail: '',
          gaurdianPhone: 0,
          UserCreated: 'Survey'
        });

      case 'interviewers':
        return this.fb.group({
          interviewerFirstName: '',
          interviewerLastName: '',
          interviewerEmail: '',
          interviewerPhone: 0,
          UserCreated: 'Survey'
        });

      case 'additional':
        return this.fb.group({
          additionalFirstName: '',
          additionalLastName: '',
          additionalEmail: '',
          additionalPhone: 0,
          UserCreated: 'Survey'
        });

      default:
        throw console.error("Not a valid member");

    }

  }

  getAsFormArray(member: string): FormArray {
    let memberArray: FormArray = this.infoForm.get(member) as FormArray
    if (memberArray === null) throw console.error(`${member} Not Found`);

    return memberArray

   }

  addMember(member: string): void {
    this.infoForm.addControl(member,this.getAsFormArray(member).push(this._newMember(member)))
    console.log(`Added: ${member}`)
  }

  removeMember(member: string, index: number): void {
    let temporary: FormArray = this.getAsFormArray(member)
    temporary.removeAt(index)
    this.infoForm.setControl(member, temporary)
    console.log(`Removed: ${member}`)
  }

  Yes(): void { this.surveyAnswer = 'Yes'; }

  onSubmit(): void {

    console.log(this.infoForm.get('juvenileFirstName'))
    console.log(this.infoForm.get('juvenileLastName'))
    console.log(this.infoForm.get('surveyAnswer'))
    console.log(this.getAsFormArray('gaurdians').controls)
    console.log(this.getAsFormArray('interviewers').controls)
    console.log(this.getAsFormArray('additional').controls)

    this.infoForm.setControl('surveyAnswer', new FormControl(this.surveyAnswer))

    this.infoJson = (JSON.parse(JSON.stringify(this.infoForm.getRawValue())))
    console.log(this.infoJson)

    this.DataService.info = this.infoJson

  }

}
