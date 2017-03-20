import { Component, OnInit, Input } from '@angular/core';

import { UserInfoModel } from '../index';
@Component({
    selector: 'ds-admin-user-viewer-option',
    templateUrl: './viewer-option.component.html',
    styleUrls: ['shared/viewer-option.component.css']
})

export class ViewerOptionComponent implements OnInit {

    @Input() userDetail: UserInfoModel;
    constructor() { }
    ngOnInit() {

    }

}
