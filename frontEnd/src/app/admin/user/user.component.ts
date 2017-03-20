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
       console.log(this.userDetail);
    }
}
