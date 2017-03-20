import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'primeng/primeng';
import { UserInfoModel } from '../index';

@Component({
  selector: 'ds-admin-user-access',
  templateUrl: './user-access.component.html'
})
export class UserAccessComponent implements OnInit {

   @Input() userDetail: UserInfoModel;
  constructor() {
  }

  public ngOnInit() {

  }
}



