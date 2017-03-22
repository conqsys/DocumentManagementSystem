import { Component, OnInit, Input } from '@angular/core';
import { UserInformationModel } from './shared/user-information.model';
import { Message } from 'primeng/primeng';

import { UserInfoModel } from '../index';
@Component({
  selector: 'ds-admin-user-info',
  templateUrl: './user-information.component.html',
   styles: ['.ui-dropdown.ui-widget.ui-state-default.ui-corner-all.ui-helper-clearfix{width:100% !important}']
})
export class UserInformationComponent implements OnInit {

  private usersList: UserInformationModel[] = [];
  private errorMsg: Message[] = [];
  private timeZones = [];
  selectedCity: string;
  confirmPassword: string;

  @Input() userDetail: UserInfoModel;

  constructor() {
    this.timeZones.push({ label: '(UTC-05:00) Eastern Standard Time', value: 'Eastern' });
    this.timeZones.push({ label: '(UTC-10:00) Western Standard Time', value: 'Western' });
    this.timeZones.push({ label: '(UTC-08:00) Northern Standard Time', value: 'Northern' });
    this.timeZones.push({ label: '(UTC-01:00) Southern Standard Time', value: 'Southern' });
  }
  public ngOnInit() {

  }
}
