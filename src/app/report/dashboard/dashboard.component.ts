import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/primeng';
import { ReportModel } from '../shared/report.model';
import { Router } from '@angular/router';

@Component({
  selector: 'sp-report-dashboard',
  templateUrl: './dashboard.component.html',
})

export class ReportDashboardComponent implements OnInit {

  private reportInfo: ReportModel;
  private errorMsg: Message[] = [];

  constructor(private router: Router) {
  }

  ngOnInit() {

  }

  public reportClickedEvent(id) {
    let link = ['report/' + id];
    this.router.navigate(link);
  }

}
