/*Import Node Modules  */
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { Ng2FilterPipeModule } from 'ng2-filter-pipe';
import { XHRBackend } from '@angular/http';
import { DataTableModule } from 'angular2-datatable';
import { ModalModule } from 'angular2-modal';
import { BootstrapModalModule } from 'angular2-modal/plugins/bootstrap';
import { Angular2DataTableModule } from 'angular2-data-table';
import { Http, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { LocalStorageService, LocalStorageModule } from 'angular-2-local-storage';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TooltipModule } from 'ng2-tooltip'; /* for tooltip */
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { DragulaModule } from 'ng2-dragula/ng2-dragula'; /* for drag and drop grid rows */
import { PopoverModule } from 'ngx-popover'; /* for for popver and toolip */
import { AlertModule } from 'ng2-bootstrap'; /* bootstrap components start */
import { InterceptorService } from 'ng2-interceptors';


/*Import PrimeNg Modules */

import {
  DropdownModule, TabViewModule, CheckboxModule,
  PanelModule,
  MultiSelectModule,
  DataTableModule as PrimeDataTableModule,
  SharedModule,
  CalendarModule,
  AccordionModule,
  ButtonModule,
  InputTextModule,
  MenubarModule,
  RadioButtonModule,
  FileUploadModule,
  DialogModule,
  DataGridModule,
  ToggleButtonModule,
  InputTextareaModule,
  PasswordModule,
  ContextMenuModule,
  AutoCompleteModule,
  GrowlModule
} from 'primeng/primeng';


/* Import Initial Modules  */

import { AppComponent } from './app.component';
import { routing } from './app.routing';
import { HttpInterceptor } from './shared/httpInterceptor';
import { HomeComponent } from './home/index';
import { CustomHttp } from './interceptor/customhttp';

/* pipe */

import { OrderBy } from './shared/pipe/orderby.pipe';

/* sp-app components */

import { LoginComponent } from './login/login.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { DashboardViewComponent } from './dashboard/dashboard-view/dashboard-view.component';
import { logoutComponent } from './shared/logout-modal/logout-modal.component';


// for Import modules

import { ImportComponent } from './import/import.component';

// for Report modules

import { ReportDashboardComponent } from './report/dashboard/dashboard.component';
import { ReportComponent } from './report/select-report/select-report.component';

/* for Reset Password */

import { ResetPasswordService } from './reset-password/shared/reset-password.service';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { confirmationModalComponent } from './shared/confirmation-modal/confirmation-modal.component';


/* admin user */

import {
  AdminUsersComponent,
  WorkflowComponent,
  SearchOptionComponent,
  ViewerOptionComponent,
  UserAccessComponent,
  UserInformationComponent,
  DocumentLevelSecurityComponent,
  UserService
} from './admin/user';
import { FileCabinetComponent } from './admin/file-cabinet/file-cabinet.component';


/* Import services */

import { LoginService } from './login/shared/login.service';
import { AuthGuard } from './guard/index';
import { PubSubService } from './interceptor/pub-service';
import { MasterService } from './shared/services/master/master.service';
import { AuthService } from './shared/services/otherServices/auth.service';
import { FileCabinateService } from './admin/file-cabinet/shared/file-cabinate.service';


let localStorageServiceConfig = {
  prefix: 'my-app',
  storageType: 'sessionStorage'
};

export function interceptorFactory(xhrBackend: XHRBackend, requestOptions: RequestOptions) {
  let service = new InterceptorService(xhrBackend, requestOptions);
  return service;
}

export function httpFactory(backend: XHRBackend, defaultOptions: RequestOptions, pubsub: PubSubService) {
  let service = new CustomHttp(backend, defaultOptions, pubsub);
  return service;
}

export function htttpInterceptor(xhrBackend: XHRBackend,
  requestOptions: RequestOptions,
  router: Router,
  pubsub: PubSubService,
  localStorageService: LocalStorageService) {
  let service = new HttpInterceptor(xhrBackend,
    requestOptions,
    router,
    pubsub,
    localStorageService);
  return service;
}

@NgModule({

  declarations: [
    AppComponent,
    LoginComponent,
    DashboardViewComponent,
    ResetPasswordComponent,
    LoadingSpinnerComponent,
    logoutComponent,
    confirmationModalComponent,
    HomeComponent,
    ImportComponent,
    ReportDashboardComponent,
    ReportComponent,
    AdminUsersComponent,
    WorkflowComponent,
    SearchOptionComponent,
    ViewerOptionComponent,
    UserAccessComponent,
    UserInformationComponent,
    DocumentLevelSecurityComponent,
    OrderBy,
    FileCabinetComponent
  ],
  entryComponents: [
    logoutComponent,
    confirmationModalComponent
  ],

  imports: [

    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    DragulaModule,
    Angular2DataTableModule,
    SharedModule,
    AlertModule,
    CheckboxModule,
    PanelModule,
    TabViewModule,
    DataTableModule,
    DropdownModule,
    PrimeDataTableModule,
    MultiSelectModule,
    NgbModule.forRoot(),
    ModalModule.forRoot(),
    BootstrapModalModule,
    TooltipModule,
    ToastModule.forRoot(),
    PopoverModule,
    AccordionModule,
    ButtonModule,
    InputTextModule,
    MenubarModule,
    RadioButtonModule,
    FileUploadModule,
    DialogModule,
    DataGridModule,
    ToggleButtonModule,
    InputTextareaModule,
    PasswordModule,
    ContextMenuModule,
    AutoCompleteModule,
    GrowlModule,
    Ng2FilterPipeModule,
    CalendarModule,
    LocalStorageModule.withConfig({
      prefix: 'my-app',
      storageType: 'localStorage'
    })
  ],
  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    {
      provide: InterceptorService,
      useFactory: interceptorFactory,
      deps: [XHRBackend, RequestOptions]
    },
    {
      provide: Http,
      useFactory: httpFactory,
      deps: [XHRBackend, RequestOptions, PubSubService]
    },
     AuthGuard,
    PubSubService,
    MasterService,
    LocalStorageService,
    AuthService,
    ResetPasswordService,
    LoginService,
    FileCabinateService,
    UserService,
    {
      provide: Http,
      useFactory: htttpInterceptor,

      deps: [XHRBackend, RequestOptions, Router, PubSubService, LocalStorageService]
    }
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class AppModule { }








