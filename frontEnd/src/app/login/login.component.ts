import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { LoginModel } from './shared/login.model';
import { AuthService } from '../shared/services/otherServices/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { PubSubService } from '../interceptor/pub-service';

import { LoginService } from './shared/login.service';
import { Message } from 'primeng/primeng';


@Component({
  selector: 'sp-login',
  templateUrl: './login.component.html'
})

export class LoginComponent implements OnInit {

  private isLoginDisplay: boolean = true;
  private emailFocus: boolean = false;
  private userNameFocus: boolean = false;
  private message: string = '';
  private loginModel: LoginModel;
  private errorMsg: Message[] = [];
  private error: string = '';
  private returnUrl: string;
  private isResetPasswordDone: boolean = false;
  constructor(private loginService: LoginService,
    private authService: AuthService,
    private localStorageService: LocalStorageService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private toastr: ToastsManager,
    public pubsub: PubSubService) {

    this.loginModel = new LoginModel();
  }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  private forgotPassword(): void {
    this.isLoginDisplay = false;
    this.emailFocus = true;
  }

  private loginAccount(): void {
    this.isLoginDisplay = true;
    this.message = '';
    this.loginModel.email = '';
    this.userNameFocus = true;
  }
  private validateEmail(email: string) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
  }
  private getResetLink(): void {

    this.errorMsg = [];
    this.error = '';
    if (this.isResetPasswordDone === false) {
      if (this.loginModel.emailId === '') {
        this.error += 'Please provide email ' + '<br>';
      } else if (!this.validateEmail(this.loginModel.emailId)) {
        this.error += 'Email inCorrect please use format like example@xyz.com';
      }
      if (this.error.length > 0) {
        this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: this.error });
        return;
      }
      this.isResetPasswordDone = true;
      this.loginService.sendResetPasswordEmail(this.loginModel.emailId)
        .then(result => {

          if (result.status !== 400) {
            this.errorMsg.push({ severity: 'success', summary: '', detail: 'Reset link send on ' + '<br>' + this.loginModel.emailId });
            this.isLoginDisplay = true;
            this.isResetPasswordDone = false;

          } else {
            this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: 'User does not exists on this email ' + '<br>' + this.loginModel.emailId });
            this.isResetPasswordDone = false;
          }

        });
    }

  }

  private login(): void {
    // let self = this;
    // messageService.showPleaseWait();
    this.authService.login(this.loginModel).then((response) => {
      // console.log(self.loginModel);
      this.localStorageService.set('authorization', 'Bearer ' + response.access_token);
      // let isResetPasswordRequired = JSON.parse(response.IsResetPasswordRequired);
      this.localStorageService.set('sessionData',
        {
          // AccountID: response.AccountID, ImageName: response.ImageName,
          // IsSuperUser: JSON.parse(response.IsSuperUser), userId: (response.userId),
          Name: response.Name, userName: response.userName,userId: (response.User.Id)
          // IsResetPasswordRequired: JSON.parse(response.IsResetPasswordRequired)
        });

      this.localStorageService.set('dashboardStateData', {
        companyId: 0,
        currentDashboardTabState: 1
        , isTabApproveInvoice: false
      });

      this.localStorageService.set('routeData', {
        prevoiusRoute: 'login',
      });

      this.router.navigate([this.returnUrl]);

    },
      (err) => {
        // messageService.hidePleaseWait();
        this.message = err.error_description;
        if (this.message === undefined) {
          this.message = 'Login failed for this user';
        }
      });
  }
}
