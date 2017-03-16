import { Component, OnInit } from '@angular/core';
import { ResetPasswordService } from './shared/reset-password.service';
import { ResetPasswordModel } from './shared/reset-password.model';
import { Router } from '@angular/router';
import { Message } from 'primeng/primeng';
import { AppComponent } from '../../app/app.component';
import { BaseComponent } from '../../app/base.component';
import { LocalStorageService } from 'angular-2-local-storage';
import { Cookie } from 'ng2-cookies';
@Component({
    selector: 'sp-reset-password',
    templateUrl: './reset-password.component.html'
})
export class ResetPasswordComponent {
    private resetPasswordDetail: ResetPasswordModel = new ResetPasswordModel();
    private errors: Array<any> = [];
    private errorHeader: string;
    newPwdFocus: boolean = false;
    confirmPwdFocus: boolean = false;
    private errorMsg: Message[] = [];
    private error: string;
    constructor(

        private resetPasswordService: ResetPasswordService,
        protected router: Router,
        protected localStorageService: LocalStorageService,

    ) {  }
    private resetPassword(): void {

        this.errorMsg = [];
        this.error = '';
        if (this.resetPasswordDetail.oldPassword === '' || this.resetPasswordDetail.oldPassword === undefined) {
            this.error += 'Old password can not be blank' + '<br>';
        }
        if (this.resetPasswordDetail.newPassword === '' || this.resetPasswordDetail.newPassword === undefined) {
            this.error += 'New password can not be blank' + '<br>';
        }
         if (this.resetPasswordDetail.newPassword.length < 6 ) {
            this.error += 'Password must be of minimum 6 characters' + '<br>';
        }
        if (this.resetPasswordDetail.confirmPassword === '' || this.resetPasswordDetail.newPassword === undefined) {
            this.error += 'Confirm password can not be blank' + '<br>';
        }
        if ((this.resetPasswordDetail.newPassword !== this.resetPasswordDetail.confirmPassword) || (this.resetPasswordDetail.newPassword != undefined && this.resetPasswordDetail.newPassword === undefined)) {
            this.error += 'New password and confirm password mismatched ' + '<br>';
        }
        if (this.error.length > 0) {
            this.errorMsg.push({ severity: 'error', summary: 'Warn Message', detail: this.error });
            return;
        }

        this.resetPasswordService.resetPassword(this.resetPasswordDetail.oldPassword, this.resetPasswordDetail.newPassword)
            .then(result => {
                if (result.status !== 400) {
                    this.errorMsg.push({ severity: 'success', summary: '', detail: 'Password changed..!!' });
                    this.localStorageService.remove('authorization');
                    this.localStorageService.remove('sessionData');
                    let link = ['/login'];
                    this.router.navigate(link);
                } else {
                    this.errorMsg.push({ severity: 'error', summary: '', detail: 'Old password mismatched' });
                }
            });

    }

}



