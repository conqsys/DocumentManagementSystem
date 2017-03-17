import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { ApiUrl } from '../../../config.component';
import { FileCabinateModel } from './file-cabinate.model';
import { PromiseHandler, PostObjectResponseModel } from '../../../shared/models/base-data.model';

@Injectable()
export class FileCabinateService {

  constructor(private http: Http) { }

  /* save function of file cabinate */
  public saveFileCabinate(fileCabinate: Array<FileCabinateModel>): Promise<any> {
    return new PromiseHandler<PostObjectResponseModel<FileCabinateModel>>(this.http
      .post(ApiUrl.baseUrl + 'Import/', fileCabinate)
      .toPromise());
  }

}
