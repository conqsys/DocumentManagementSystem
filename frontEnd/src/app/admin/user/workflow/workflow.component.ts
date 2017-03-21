import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

import { UserModel } from '../../../shared/models/user.model';
import { MasterService } from '../../../shared/services/master/master.service';
import { QueueModel } from './index';
import { WorkflowService } from './shared/workflow.service';
import { UserInfoModel } from '../index';
@Component({
  selector: 'ds-admin-user-workflow',
  templateUrl: './workflow.component.html',
  styleUrls: ['shared/workflow.component.css']
})

export class WorkflowComponent implements OnInit {


  @Input() userDetail: UserInfoModel;
  private checked: boolean = false;
  private selectedUser: number;
 // private queues: Array<any> = new Array<any>();
  private selectedIndex: number = -1;
  private errorMsg: Array<any> = [];
  private users: Array<UserModel> = new Array<UserModel>();
  constructor(private http: Http,
    router: Router,
    private activatedRoute: ActivatedRoute,
    private masterService: MasterService,
    private workflowService: WorkflowService
  ) {
    // this.queues = [];

  }
  ngOnInit() {
    this.getUserList();

  }
  private getUserList() {
    let enabledRequired: Boolean = false;
    this.masterService.getUserList(enabledRequired).then(result => {
      this.users = result.data;
      this.users.splice(0, 0, new UserModel());
      this.users.map((item: any) => {
        item.label = item.userName;
        item.value = item.userId;
      });
      this.getQueues();
    }).catch(err => {
      this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: err.ValidatonResult.errorMessage });
    });
  }
  private getQueues(): void {
    this.workflowService.getUserDetail().then(res => {
     this.userDetail.queues = res;
     console.log(this.userDetail.queues);
    }).catch(err => {
      this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: err.ValidatonResult.errorMessage });
    });
  }

  private onSelectQueue(queue): void {
    this.userDetail.queues.map(res => {
      if (res.id === queue.id) {
        res.isCheck = !res.isCheck;
      }
    });
  }
}
