import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
@Component({
    selector: 'ds-admin-user',
    templateUrl: './user.component.html',
styles: [`li.ui-state-default.ui-corner-top.ui-tabview-selected.ui-state-active{background: #d21414 }`]
})

export class AdminUsersComponent implements OnInit {

    constructor(private http: Http,
        router: Router,
        private activatedRoute: ActivatedRoute ) {

    }
    ngOnInit() {

    }
}
