import { Component, OnInit } from '@angular/core';
// import { UserInfoService } from '../master/user/shared/user.service';
// import { ClientService } from '../master/client/shared/client.service';
// import { UserInfoModel } from '../master/user/shared/user-info.model';
// import { ClientItem } from '../master/client/shared/client.model';
import { Message } from 'primeng/primeng';
import { ImportInfoModel } from './shared/import.model';

@Component({
  selector: 'sp-import',
  templateUrl: './import.component.html',
})

export class ImportComponent implements OnInit {
  private errorMsg: Message[] = [];
  private import: ImportInfoModel;
  // private users: Array<UserInfoModel> = [];
  // private clients: Array<ClientItem> = [];
  private uploadedFiles: Array<any> = [];

  constructor(// private userService: UserInfoService, private clientService: ClientService
  ) {
    this.import = new ImportInfoModel();
  }

  ngOnInit() {
    // this.getUserList();
  }

  // private getUserList() {
  //   let enabledRequired: Boolean = false;
  //   this.userService.getUserList(enabledRequired).then(result => {
  //     this.users = result.data;
  //     this.users.map((item: any) => {
  //       item.label = item.userName;
  //       item.value = item.userId;
  //     });
  //     this.getClients();
  //   }).catch(err => {
  //     this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: err.ValidatonResult.errorMessage });
  //   });
  // }


  // private getClients() {
  //   let enabledRequired: Boolean = false;
  //   this.clientService.getClientItems(enabledRequired).then(result => {
  //     this.clients = result.data;
  //     this.clients.map((item: any) => {
  //       item.label = item.clientAcronym;
  //       item.value = item.id;
  //     });
  //   }).catch(err => {
  //     this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: err.ValidatonResult.errorMessage });
  //   });
  // }

  private onUpload(image) {
    for (let file of image.files) {
      this.uploadedFiles.push(file);
    }

    let imageUrl: any;
    let imageObject = image.files[0];
    // this.userDetail.imageName = imageObject.name;
    // this.userDetail.imageType = imageObject.type;
    let reader = new FileReader();
    reader.readAsDataURL(imageObject);
    reader.onload = function () {
      imageUrl = reader.result;
    };

    //   setTimeout(() => {
    //     this.userDetail.imageSource = imageUrl;
    //   }, 10);

    this.errorMsg = [];
    this.errorMsg.push({ severity: 'info', summary: 'File Uploaded', detail: '' });
  }

  private saveImport() {

  }

}
