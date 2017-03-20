import { Component, OnInit, Input } from '@angular/core';

import { UserInfoModel } from '../index';
@Component({
  selector: 'ds-admin-user-level-security',
  templateUrl: './document-level-security.component.html',
  styleUrls:['./shared/document-level-security.component.css']
})
export class DocumentLevelSecurityComponent implements OnInit {

   @Input() userDetail: UserInfoModel;
  constructor() {
  
   }
  public ngOnInit() {
  }

  private getUsers() {
    // let enabledRequired: Boolean = false;
  }
}



