import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DataService } from './../../data-service.service';
import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { last } from 'rxjs'

@Component({
  selector: 'app-api-interace',
  templateUrl: './api-interace.component.html'
})

export class ApiInteraceComponent implements OnInit, OnChanges {

  private _postURL: string;

  private _userInfo: any;
  private _qandas;

  private _dataPost;
  private _submitted: boolean;

  constructor (
    private _DataService: DataService,
    private _http: HttpClient
  )
  {

    this._userInfo = {
      JfirstName: '',
      JlastName:'',
      gaurdians: [{
        GfirstName: '',
        GlastName: '',
        Gemail: '',
        Gphone: '',
        UserCreated: 'Survey'
      }],
      interviewers: [{
        IfirstName: '',
        IlastName: '',
        Iemail: '',
        Gphone: '',
        UserCreated: 'Survey'
      }],
      additional: [{
        AfirstName: '',
        AlastName: '',
        Aemail: '',
        Gphone: '',
        UserCreated: 'Survey'
      }]
    };

    this._qandas = [{
      surveyId: 0,
      questionId: 0,
      answer: '',
      userCreated: ''
      }];

    this._dataPost = {}
    this._submitted = false
    this._postURL = 'https://localhost:7071/api/SurveyInfoes'

  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['_submitted'].currentValue)
      this._postData(this._dataPost)
  }

  ngOnInit(): void {

    this._DataService.info.subscribe(info => {

      console.log('api userInfo: ', info)
      this._userInfo = info

    });

    this._DataService.qandas.subscribe(qanda => {

      console.log('api qanda: ', qanda)
      this._qandas = qanda


    });

    this._DataService.isSubmitted.pipe(last()).subscribe(s => {

      this._submitted = s
      console.log(s)

      this._dataPost = { ...this._userInfo, ...this._qandas }

    });

    if (this._submitted) {
      this._postData(this._dataPost)
    }

  }

  private _postData(data: any) {

    console.log('posting data: ', data)

    let header = new HttpHeaders()
    header.append('Content-type', 'application/json')
    header.append('accept', 'text/plain')
    header.append('Access-Control-Allow-Origin', 'http://localhost:4200')
    this._http.post(`${this._postURL}?apikey=U3VydmV5S2V5Rm9ySnV2ZW5pbGVTdXJ2ZXk%3D`, this._dataPost).subscribe(msg => {

      console.log('postMessage: ', msg)
      this._http.get(`${this._postURL}/${JSON.parse(JSON.stringify(msg)).sdid}`).subscribe(data =>

        console.log('From DB: ', data)

        );

    });

  }

}
