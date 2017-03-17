import { Injectable } from '@angular/core';
import { Http, URLSearchParams, RequestOptionsArgs, RequestOptions, Headers } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { ApiUrl } from '../../../config.component';
import { LocalStorageService } from 'angular-2-local-storage';


@Injectable()

export class AuthService {

    constructor(private http: Http,
        private localStorageService: LocalStorageService) {
    }

    public login(loginData): Promise<any> {
        // var data = 'grant_type=password&username=' + loginData.userName + '&password=' + loginData.password;
        if (loginData.useRefreshTokens) {
            // data = data + "&client_id=" + 'IOneApp';//ngAuthSettings.clientId;
        }

        let options = new RequestOptions();
        options.headers = new Headers();
        options.headers.append('Content-Type', 'application/x-www-form-urlencoded');
        return this.http.post(ApiUrl.loginUrl + 'login/'  + loginData.userName +'/'+ loginData.password, options).toPromise()

            .then(response =>
                response.json())
            .catch(this.handleError);
    };


    private handleError(error: any): Promise<any> {
        // console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

    public sendResetPasswordEmail(loginData) {
        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;
        let params: URLSearchParams = new URLSearchParams();
        params.set('email', loginData.email);
        return this.http
            .get(ApiUrl.baseUrl + 'api/resetpassword/sendmail/', { search: params })
            .toPromise()
            .then(response =>
                response.json());
    }
}