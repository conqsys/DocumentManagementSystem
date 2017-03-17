import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
@Component({
    selector: 'ds-admin-user-search-option',
    templateUrl: './search-option.component.html',
    styleUrls: ['shared/search-option.component.css']
})

export class SearchOptionComponent implements OnInit {

 checked: boolean = false;
constructor() {}
ngOnInit() {

    }

}
