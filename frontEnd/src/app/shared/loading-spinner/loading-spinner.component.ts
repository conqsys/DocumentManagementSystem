import { Component, OnInit,Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { BaseComponent } from '../../base.component';
import { LocalStorageService } from 'angular-2-local-storage';
import { Router } from '@angular/router';
import { Location } from '@angular/common';


@Component({
  selector: 'sp-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls:  ['css/spinner.css']
})

  
export class LoadingSpinnerComponent extends BaseComponent implements OnInit  {

  @Input() showLoader ;
    constructor(
    private http: Http,
    localStorageService: LocalStorageService,
    router: Router,
    private activatedRoute: ActivatedRoute,
    private location: Location
  ) {
    super(localStorageService, router);
  }
   ngOnInit() {
  }
}