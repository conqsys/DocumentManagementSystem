import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

import { UserModel } from '../../../shared/models/user.model';
import { MasterService } from '../../../shared/services/master/master.service';
@Component({
  selector: 'ds-admin-user-workflow',
  templateUrl: './workflow.component.html',
  styleUrls: ['shared/workflow.component.css']
})

export class WorkflowComponent implements OnInit {


  private checked: boolean = false;
  private selectedUser: number;
  private queues: Array<any> = [];
  private selectedIndex: number = -1;
  private errorMsg: Array<any> = [];
  private users: Array<UserModel> = new Array<UserModel>();
  constructor(private http: Http,
    router: Router,
    private activatedRoute: ActivatedRoute,
    private masterService: MasterService
  ) {
    this.queues = [];

    this.queues.push({ label: 'queue 6', value: '6', isCheck: false });
    this.queues.push({ label: 'queue 1', value: '1', isCheck: false });
    this.queues.push({ label: 'queue 2', value: '2', isCheck: false });
    this.queues.push({ label: 'queue 3', value: '3', isCheck: false });
    this.queues.push({ label: 'queue 4', value: '4', isCheck: false });
    this.queues.push({ label: 'queue 5', value: '5', isCheck: false });
    this.queues.push({ label: 'queue 7', value: '7', isCheck: false });
    this.queues.push({ label: 'queue 8', value: '8', isCheck: false });
    this.queues.push({ label: 'queue 9', value: '9', isCheck: false });
    this.queues.push({ label: 'queue 10', value: '10', isCheck: false });
  }
  ngOnInit() {
    this.getUserList();

  }
  private getUserList() {
    let enabledRequired: Boolean = false;
    this.masterService.getUserList(enabledRequired).then(result => {
      this.users = result.data;
      this.users.splice(0, 0, new UserModel( ));
      this.users.map((item: any) => {
        item.label = item.userName;
        item.value = item.userId;
      });
      // this.getClients();
    }).catch(err => {
      this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: err.ValidatonResult.errorMessage });
    });
  }

  onSelectQueue(queue): void {
    this.queues.map(res => {
      if (res.value === queue.value) {
        res.isCheck = !res.isCheck;
      }
    });
  }
}
