import { Http, Response, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';
import { ApiUrl } from '../../../config.component';
import {
  ArrayResponseModel,
  PromiseHandler
} from '../../models/base-data.model';

import { UserModel } from '../../models/user.model';

import 'Rxjs/Rx';
@Injectable()

export class MasterService {
  constructor(private http: Http) { }

  public getUserList(enabledRequired): Promise<ArrayResponseModel<UserModel>> {
    let promise = this.http
      .get(ApiUrl.baseUrl + 'User/' + enabledRequired + '/list')
      .toPromise();
    return new PromiseHandler<ArrayResponseModel<UserModel>>(promise);
  }

}
