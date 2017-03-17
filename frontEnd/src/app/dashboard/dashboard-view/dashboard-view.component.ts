import { Component, OnInit, } from '@angular/core';
import { BaseComponent } from '../../base.component';
import { LocalStorageService } from 'angular-2-local-storage';
import { Router } from '@angular/router';
import { Interceptor } from 'ng2-interceptors';
import { PubSubService } from '../../interceptor/pub-service';

@Component({
  selector: 'sp-dashboard',
  templateUrl: './dashboard-view.component.html',
})


export class DashboardViewComponent extends BaseComponent implements OnInit, Interceptor {

  private companyId: number = 0;
  private invoiceState: number = 1;
  private totalItems: number = 0;
  private invoiceForValidApprovalCount = 0;
  private showLoader: boolean = false;
  constructor(
    localStorageService: LocalStorageService,
    router: Router,
    public pubsub: PubSubService
  ) {
    super(localStorageService, router);
    this.companyId = 0;
    this.invoiceState = 1;
    this.totalItems = 0;
    this.invoiceForValidApprovalCount = 0;
  }

  ngOnInit() {
    this.pubsub.beforeRequest.subscribe(data => this.showLoader = true);
    this.pubsub.afterRequest.subscribe(data => this.showLoader = false);
  }

}
