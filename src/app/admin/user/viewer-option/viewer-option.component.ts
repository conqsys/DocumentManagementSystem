import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { Router } from '@angular/router';
@Component({
    selector: 'ds-admin-user-viewer-option',
    templateUrl: './viewer-option.component.html',
    styleUrls: ['shared/viewer-option.component.css']
})

export class ViewerOptionComponent implements OnInit {

    checked: boolean = false;
    constructor() { }
    ngOnInit() {

    }

}
