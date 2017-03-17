import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';


export class BaseComponent {
  user: any;
  sessionDetails: any;
  disableMenu: Object = { 'display': 'block' };
  disableSideBar: Object = { 'display': '' };
  dashboardState: any;
  currentDashboardTabState: string = '';
  prevoiusRouteState: any;

  constructor(protected localStorageService: LocalStorageService,
    protected router: Router
  ) { }

}
