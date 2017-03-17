import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import { Modal, BSModalContextBuilder } from 'angular2-modal/plugins/bootstrap';
import { OverlayConfig } from 'angular2-modal';
import {
  logoutModalContext,
  logoutComponent
} from '../shared/logout-modal/logout-modal.component';

@Component({
  selector: 'sp-home',
  templateUrl: './home.component.html',

})

export class HomeComponent implements OnInit {

  private user: any = {};
  constructor(
    private localStorageService: LocalStorageService,
    private router: Router,
    private modal: Modal
  ) {

    if (this.localStorageService.get('authorization') && this.localStorageService.get('authorization') !== 'undefined') {
      this.user = this.localStorageService.get('sessionData');
    }
  }

  ngOnInit() {

  }
  openlogoutModal() {
    const builder = new BSModalContextBuilder<logoutModalContext>(
      { num1: 2, num2: 3 } as any,
      undefined,
      logoutModalContext
    );

    let overlayConfig: OverlayConfig = {
      context: builder.isBlocking(false).toJSON()
    };

    const dialog = this.modal.open(logoutComponent, overlayConfig);
    dialog.then((resultPromise) => {
      return resultPromise.result.then((result) => {

        if (result === 'logout') {
          this.logOut();
        }
      }, () => console.log('  '));
    });
  }

  private logOut() {

    this.localStorageService.remove('authorization');
    this.localStorageService.remove('sessionData');
    let link = ['/login'];
    this.router.navigate(link);

  }

  private onPasswordReset(): void {
    let link = ['/resetPassword'];
    this.router.navigate(link);
  }
}
