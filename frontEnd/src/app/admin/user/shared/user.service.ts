import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { ApiUrl } from '../../../config.component';

@Injectable()

export class UserService {

    constructor(private http: Http) {
    }

    public getUserDetail(userId): Promise<any> {
        return this.http.get(ApiUrl.baseUrl + 'userDetail/' + userId).toPromise()

            .then(response =>
                response.json())
            .catch(this.handleError);
    };

    public saveUserDetail(userDetail): Promise<any> {
        return this.http.post(ApiUrl.baseUrl + 'userDetail/' , JSON.stringify(userDetail)).toPromise()

            .then(response =>
                response.json())
            .catch(this.handleError);
    };




    private handleError(error: any): Promise < any > {
    // console.error('An error occurred', error);
    return Promise.reject(error.message || error);
}
}