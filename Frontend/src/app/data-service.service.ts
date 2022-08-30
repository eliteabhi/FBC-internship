import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class DataService {

  private _info: BehaviorSubject<any>;
  private _qandas: BehaviorSubject<any>;
  private _isSubmitted: ReplaySubject<boolean>;

  constructor() {

    this._info = new BehaviorSubject<any>(null)
    this._qandas = new BehaviorSubject<any>(null)
    this._isSubmitted = new ReplaySubject<boolean>(1)

  }

  public getAll(): Observable<any[]> {

    var allData: BehaviorSubject<Observable<any>[]> = new BehaviorSubject<any[]>([])
    let temp: any[] = []

    temp.push(this._info.asObservable())
    temp.push(this._qandas.asObservable())

    allData.next(temp)

    return allData.asObservable()

  }

  public set info(info: any) {
    this._info.next(info)
    console.log('DataService setInfo: ', info)
  }

  public get info(): Observable<any> {
    return this._info.asObservable()
  }

  public set qandas(qanda: any) {
    this._qandas.next(qanda);
    console.log('DataService setQandas: ', qanda)
  }

  public get qandas(): Observable<any> {
    return this._qandas.asObservable()
  }

  public get isSubmitted(): Observable<boolean> {
    return this._isSubmitted.asObservable()
  }

  public set isSubmitted(value: any) {
    this._isSubmitted.next(value)
    this._isSubmitted.complete()

  }

  public completeSubmit(prop: string) {

    switch (prop) {
      case 'isSubmitted':
        this._isSubmitted.complete()
        break;

      case 'info':
        this._info.complete()
        break;
    
      default:
        console.error('not a valid property: ', prop)
        break;
    }

  }

}
