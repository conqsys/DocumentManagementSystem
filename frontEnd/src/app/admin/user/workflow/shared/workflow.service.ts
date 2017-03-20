import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { ApiUrl } from '../../../../config.component';

@Injectable()

export class WorkflowService {

    constructor(private http: Http) {
    }

    public getUserDetail(): Promise<any> {
        return this.http.get(ApiUrl.baseUrl + 'Queue/getListOfUsers').toPromise()

            .then(response =>
                response.json())
            .catch(this.handleError);
    };


    private handleError(error: any): Promise<any> {
        // console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}