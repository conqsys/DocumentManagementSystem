import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/primeng';
import { ReportModel } from '../shared/report.model';
import { ActivatedRoute } from '@angular/router';
// import { UserInfoService } from '../../master/user/shared/user.service';
// import { UserInfoModel } from '../../master/user/shared/user-info.model';
// import { ClientService } from '../../master/client/shared/client.service';
// import { ClientItem } from '../../master/client/shared/client.model';

@Component({
  selector: 'sp-report-select-report',
  templateUrl: './select-report.component.html',
})

export class ReportComponent implements OnInit {

  private reportInfo: ReportModel;
  // private users: Array<UserInfoModel> = [];
  // private clients: Array<ClientItem> = [];
  private errorMsg: Message[] = [];
  private display: boolean = false;
  private reportId: number = 0;
  private modelWidth: string = '500';

  constructor( // private userService: UserInfoService,    private clientService: ClientService,
    private route: ActivatedRoute) {

  }

  ngOnInit() {
    this.getParameter();
  }

  private getParameter() {
    this.route.params.subscribe(params => {
      this.reportId = Number(params['id']);
    });

    switch (this.reportId) {
      case 1:
        this.modelWidth = '850';
        break;
      case 6:
        this.modelWidth = '850';
        break;
    }
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

  private showDialog() {
    this.display = true;
  }

}
