import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './login/login.component';
import { DashboardViewComponent } from './dashboard/dashboard-view/dashboard-view.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ImportComponent } from './import/import.component';
import { ReportDashboardComponent } from './report/dashboard/dashboard.component';
import { ReportComponent } from './report/select-report/select-report.component';
import { AdminUsersComponent } from './admin/user';
import { FileCabinetComponent } from './admin/file-cabinet/file-cabinet.component';

import { AuthGuard } from './guard/index';
import { HomeComponent } from './home/index';
const APP_ROUTES: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardViewComponent },
      { path: 'admin/users', component: AdminUsersComponent },
      { path: 'resetPassword', component: ResetPasswordComponent },
      { path: 'import', component: ImportComponent },
      { path: 'report', component: ReportDashboardComponent },
      { path: 'report/:id', component: ReportComponent },
      { path: 'admin/filecabinate', component: FileCabinetComponent }
    ]
  },
  { path: 'login', component: LoginComponent }

];

export const routing = RouterModule.forRoot(APP_ROUTES);
