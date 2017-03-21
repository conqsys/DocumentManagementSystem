import { Component, OnInit, } from '@angular/core';
import { Message } from 'primeng/primeng';
import { FileCabinateModel } from './shared/file-cabinate.model';
import { FileCabinateService } from './shared/file-cabinate.service';
import { LocalStorageService } from 'angular-2-local-storage';


@Component({
  selector: 'sp-admin-file-cabinet',
  templateUrl: './file-cabinet.component.html',
})


export class FileCabinetComponent implements OnInit {

  /* variables declaration */

  private errorMessage: Message[] = [];
  private addNewCabinate: Array<any> = [];
  private fileCabinateModel: FileCabinateModel = new FileCabinateModel();
  private indexType: Array<any> = [{ label: 'Text', value: 0 }, { label: 'List', value: 1 }];
  private fileCabinates: Array<FileCabinateModel> = [];
  private hideSaveButton: boolean = true;
  private user: any = {};

  constructor(private fileCabinateService: FileCabinateService,  private localStorageService: LocalStorageService) {

     if (this.localStorageService.get('authorization') && this.localStorageService.get('authorization') !== 'undefined') {
            this.user = this.localStorageService.get('sessionData');
            this.fileCabinateModel.userId = this.user.userId;
         //   this.user = this.localStorageService.get('sessionData');
            // this.userService.getUserDetail(this.user.userId).then(res => {
            //     this.userDetail = res;
            //     console.log(this.userDetail);
            // });
        }
   }

  ngOnInit() {
    this.getFileCabinates();
  }
  /* get  file cabinates list */
  public getFileCabinates() {
    this.fileCabinateService.getFileCabinates().then(result => {
      this.fileCabinates = result.data;
    });
  }
  /* add multiple file cabinates */
  private addFileCabinate(isAddNew): boolean {
    this.hideSaveButton = false;
    let errorCount = 0;
    let errorList = '';
    if (this.addNewCabinate.length !== 0) {
      if (this.addNewCabinate[this.addNewCabinate.length - 1].indexName === '') {
        errorCount++;
        errorList += 'please fill index Name' + '<br>';
      }

      if (this.addNewCabinate[this.addNewCabinate.length - 1].defaultValue === '') {
        errorCount++;
        errorList += 'please fill default Name' + '<br>';
      }

      if (this.addNewCabinate[this.addNewCabinate.length - 1].list === ''
        && this.addNewCabinate[this.addNewCabinate.length - 1].indexType === 1) {
        errorCount++;
        errorList += 'please fill atleast one list item' + '<br>';
      }

      if (this.addNewCabinate[this.addNewCabinate.length - 1].list !== '') {
      this.addNewCabinate[this.addNewCabinate.length - 1].listValue = JSON.stringify(this.addNewCabinate[this.addNewCabinate.length - 1].list.split('\n'));
      }

      /* check default valus is exists in List items */
      if (this.addNewCabinate[this.addNewCabinate.length - 1].indexType === 1 &&
        this.addNewCabinate[this.addNewCabinate.length - 1].list !== ''
        && this.addNewCabinate[this.addNewCabinate.length - 1].defaultValue !== '') {
        let checkDefaultValueExist = 0;
        let listArray = this.addNewCabinate[this.addNewCabinate.length - 1].list.split('\n');
        listArray.forEach((element) => {
          if (element === this.addNewCabinate[this.addNewCabinate.length - 1].defaultValue) {
            checkDefaultValueExist++;
          }
        });

        if (checkDefaultValueExist === 0 && errorList === '') {
          errorCount++;
          errorList += 'Default value should be in List Items' + '<br>';
        }
      }

      /* check IndexName is already exits or not */
      if (this.addNewCabinate[this.addNewCabinate.length - 1].indexName && this.addNewCabinate.length - 1 > 0
        && this.addNewCabinate[this.addNewCabinate.length - 1].id === 0) {
        let checkIndexNameDuplicate = false;
        let count = 0;
        this.addNewCabinate.map(item => {
          if (item.indexName === this.addNewCabinate[this.addNewCabinate.length - 1].indexName) {
            count++;
            if (count > 1) {
              checkIndexNameDuplicate = true;
            }

          }
        });

        if (checkIndexNameDuplicate && errorList === '') {
          errorCount++;
          errorList += 'Index name already exists. Please enter unique name' + '<br>';
        }
      }

      if (errorCount > 0) {
        this.errorMessage.push({ severity: 'error', detail: errorList });
        return false;
      } else {
        if (isAddNew === false) {
          return true;
        }
      }
    }
    if (isAddNew === true) {
      this.addNewCabinate.push(new FileCabinateModel());
    }
  }


  /* save file cabinates */
  private saveFileCabinate() {
    if (this.addFileCabinate(false)) {
         this.fileCabinateService.saveFileCabinate(this.addNewCabinate).then(res => {
        this.errorMessage.push({ severity: 'success', summary: 'Success', detail: 'Result Saved' });
      });
    }
  }


  /* deleted selected file cabinate */
  private deleteFileCabinate(selectedFileCabinate) {
    this.fileCabinates.splice(selectedFileCabinate.id - 1, 1);
  }

}
