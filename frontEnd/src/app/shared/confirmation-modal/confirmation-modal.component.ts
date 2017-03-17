import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-2-local-storage';
import { BaseComponent } from '../../base.component';
import { DialogRef, ModalComponent, CloseGuard } from 'angular2-modal';
import { BSModalContext } from 'angular2-modal/plugins/bootstrap';

export class confirmationModalContext extends BSModalContext {
  header;
  constructor() {
    super();
  }
}

@Component({
  selector: 'sp-confirmation-modal',
  templateUrl: 'confirmation-modal.component.html'
})

export class confirmationModalComponent extends BaseComponent implements CloseGuard, ModalComponent<confirmationModalContext>, OnInit {

  context: confirmationModalContext;
  public wrongAnswer: boolean;
  private header: string = '';

  constructor(localStorageService: LocalStorageService,
    router: Router,
    public dialog: DialogRef<confirmationModalContext>) {
    super(localStorageService, router);
    this.context = dialog.context;
    this.header = this.context.header;
    dialog.setCloseGuard(this);


  }

  ngOnInit() {

  }

  closeModal(event): void {
    this.dialog.close(event);
  }
}


