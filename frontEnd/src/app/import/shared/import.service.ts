import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { ApiUrl } from '../../config.component';
import { ImportInfoModel } from './import.model';
import { PromiseHandler, PostObjectResponseModel } from '../../shared/models/base-data.model';

@Injectable()
export class ClientService {
  constructor(private http: Http) {

  }

  public saveImport(importObj: ImportInfoModel): Promise<any> {

    return new PromiseHandler<PostObjectResponseModel<ImportInfoModel>>(this.http
      .post(ApiUrl.baseUrl + 'Import/', importObj)
      .toPromise());
  }

}
