import { Component, OnInit } from '@angular/core';

import { Message } from 'primeng/primeng';
@Component({
  selector: 'ds-admin-user-level-security',
  templateUrl: './document-level-security.component.html',
  styleUrls:['./shared/document-level-security.component.css']
})
export class DocumentLevelSecurityComponent implements OnInit {

  private errorMsg: Message[] = [];
  private timeZones =[];
  constructor() {
    this.timeZones.push({label: '(UTC-05:00)Eastern Standard Time', value: {id: 1, name: 'New York', code: 'NY'}});
        this.timeZones.push({label: '(UTC-10:00) Western Standard Time', value: {id: 2, name: 'Rome', code: 'RM'}});
        this.timeZones.push({label: '(UTC-08:00) Northern Standard Time', value: {id: 3, name: 'London', code: 'LDN'}});
        this.timeZones.push({label: '(UTC-01:00) Southern Standard Time', value: {id: 4, name: 'Istanbul', code: 'IST'}});
   }
  public ngOnInit() {
  }

  private getUsers() {
    // let enabledRequired: Boolean = false;
  }
}



