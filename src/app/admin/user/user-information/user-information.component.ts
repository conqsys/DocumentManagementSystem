import { Component, OnInit } from '@angular/core';
import { UserInformationModel } from './shared/user-information.model';
import { Message } from 'primeng/primeng';
@Component({
  selector: 'ds-admin-user-info',
  templateUrl: './user-information.component.html'
})
export class UserInformationComponent implements OnInit {

  private usersList: UserInformationModel[] = [];
  private errorMsg: Message[] = [];
  private timeZones = []


  constructor() {
    this.timeZones.push({ label: '(UTC-05:00)Eastern Standard Time', value: { id: 1, name: 'New York', code: 'NY' } });
    this.timeZones.push({ label: '(UTC-10:00) Western Standard Time', value: { id: 2, name: 'Rome', code: 'RM' } });
    this.timeZones.push({ label: '(UTC-08:00) Northern Standard Time', value: { id: 3, name: 'London', code: 'LDN' } });
    this.timeZones.push({ label: '(UTC-01:00) Southern Standard Time', value: { id: 4, name: 'Istanbul', code: 'IST' } });
  }
  public ngOnInit() {

  }

  private getUsers() {
    // let enabledRequired: Boolean = false;

  }
}



