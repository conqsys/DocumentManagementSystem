import { Component, OnInit, Input } from '@angular/core';

import { UserInfoModel } from '../index';
@Component({
    selector: 'ds-admin-user-search-option',
    templateUrl: './search-option.component.html',
    styleUrls: ['shared/search-option.component.css']
})

export class SearchOptionComponent implements OnInit {

    @Input() userDetail: UserInfoModel;
    constructor() { }
    ngOnInit() {

    }

}
