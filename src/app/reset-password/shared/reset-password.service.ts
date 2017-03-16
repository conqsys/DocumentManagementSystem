import { Http, Headers, RequestOptions } from '@angular/http';
import { Injectable } from '@angular/core';
import 'Rxjs/Rx';
import { ResetPasswordModel } from './reset-password.model';
import { ApiUrl } from '../../config.component';

@Injectable()
export class ResetPasswordService {
  constructor(private http: Http) {

  }
  public getUserDetail(resetPasswordToken): Promise<ResetPasswordModel> {
    return this.http
      .get(ApiUrl.loginUrl
      + 'api/resetpassword?resetPasswordToken='
      + resetPasswordToken)
      .toPromise()
      .then(response => response.json() as ResetPasswordModel)
      .catch(error => error);
  }
  public resetPassword(oldPassowrd: string, newpassword: string) {

    return this.http
      .post(ApiUrl.baseUrl + 'User/resetpassword/' + oldPassowrd + '/' + newpassword, {})
      .toPromise()
      .then(response =>
        response)
      .catch(error => error);

  }


}
