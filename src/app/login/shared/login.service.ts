import { Injectable } from '@angular/core';
import { Http, URLSearchParams, RequestOptionsArgs, RequestOptions, Headers } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { ApiUrl } from '../../../app/config.component';
import { LocalStorageService } from 'angular-2-local-storage';


@Injectable()

export class LoginService {

    constructor(private http: Http,
        private localStorageService: LocalStorageService) {
    }

    public sendResetPasswordEmail(emailId: string) {
        return this.http
            .post(ApiUrl.baseUrl + 'ForgotPassword/resetforgetpassword/'+ emailId, {})
            .toPromise()
            .then(response =>
                response)
            // .catch(error => error);

    }
}