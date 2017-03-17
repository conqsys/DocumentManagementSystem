import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import { BaseComponent } from '../../base.component';
import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';

export class logoutModalContext extends BSModalContext {
  constructor() {
    super();
  }
}

@Component({
  selector: 'sp-logout-modal',
  templateUrl: 'logout-modal.component.html'
})
export class logoutComponent extends BaseComponent implements CloseGuard, ModalComponent<logoutModalContext>, OnInit {

  context: logoutModalContext;
  public wrongAnswer: boolean;


  constructor(localStorageService: LocalStorageService,
    router: Router,
    public dialog: DialogRef<logoutModalContext>) {
    super(localStorageService, router);
    this.context = dialog.context;
    dialog.setCloseGuard(this);
  }

  ngOnInit() { }

  closeModal(event): void {
    this.dialog.close(event);
  }

}
