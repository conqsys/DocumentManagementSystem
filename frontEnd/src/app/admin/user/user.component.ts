import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import { UserService } from './shared/user.service';
import { UserInfoModel } from './index';

@Component({
    selector: 'ds-admin-user',
    templateUrl: './user.component.html',
    styles: [`li.ui-state-default.ui-corner-top.ui-tabview-selected.ui-state-active{background: #d21414 }`]
})

export class AdminUsersComponent implements OnInit {

    private userDetail: UserInfoModel;
    private user: any = {};
    private selectedQueues: Array<any> = [];
    constructor(private http: Http,
        router: Router,
        private activatedRoute: ActivatedRoute,
        private localStorageService: LocalStorageService,
        private userService: UserService) {

    }
    ngOnInit() {
        if (this.localStorageService.get('authorization') && this.localStorageService.get('authorization') !== 'undefined') {
            this.user = this.localStorageService.get('sessionData');
            this.userService.getUserDetail(this.user.userId).then(res => {
                this.userDetail = res;
                console.log(this.userDetail);
            });
        }

    }
    private saveUserDetail(): void {
        let queue = this.userDetail.queues;
        this.userDetail.queues = [];
        queue.map(res => {
            let selectedQueue = { id:0, queueId:1 , userId: this.userDetail.userId }
            this.selectedQueues.push(selectedQueue);
            // res.queueId=res.id;
            // res.userId=this.userDetail.userId;
        })
        this.userDetail.queues = this.selectedQueues;
        console.log(this.userDetail);
        this.userService.saveUserDetail(this.userDetail).then(res => {

            console.log(res);
        })

    }
}
