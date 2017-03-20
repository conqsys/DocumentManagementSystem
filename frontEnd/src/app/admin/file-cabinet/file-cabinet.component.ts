import { Component, OnInit, } from '@angular/core';
import { Message } from 'primeng/primeng';
import { FileCabinateModel } from './shared/file-cabinate.model';
import { FileCabinateService } from './shared/file-cabinate.service';


@Component({
  selector: 'sp-admin-file-cabinet',
  templateUrl: './file-cabinet.component.html',
})


export class FileCabinetComponent implements OnInit {

  /* variables declaration */
  private list: string = '';
  private errorMessage: Message[] = [];
  private fileCabinateModel: FileCabinateModel = new FileCabinateModel();
  private indexType: Array<any> = [{ label: 'Text', value: 0 }, { label: 'List', value: 1 }];
  private fileCabinates: Array<FileCabinateModel> = [];

  constructor(private fileCabinateService: FileCabinateService) { }

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
  private addFileCabinate() {

    let errorCount = 0;
    let errorList = '';

    if (this.fileCabinateModel.indexName === '') {
      errorCount++;
      errorList += 'please fill index Name' + '<br>';
    }

    if (this.fileCabinateModel.defaultValue === '') {
      errorCount++;
      errorList += 'please fill default Name' + '<br>';
    }

    if (this.list === '' && this.fileCabinateModel.indexType === 1) {
      errorCount++;
      errorList += 'please fill atleast one list item' + '<br>';
    }

    if (this.list !== '') {
      this.fileCabinateModel.listValue = JSON.stringify(this.list.split('\n'));
    }

    /* check default valus is exists in List items */
    if (this.fileCabinateModel.indexType === 1 &&
      this.list !== '' && this.fileCabinateModel.defaultValue !== '') {
      let checkDefaultValueExist = 0;
      let listArray = this.list.split('\n');
      listArray.forEach((element) => {
        if (element === this.fileCabinateModel.defaultValue) {
          checkDefaultValueExist++;
        }
      });

      if (checkDefaultValueExist === 0 && errorList === '') {
        errorCount++;
        errorList += 'Default value should be in List Items' + '<br>';
      }
    }

    /* check IndexName is already exits or not */
    if (this.fileCabinateModel.indexName && this.fileCabinates.length > 0 && this.fileCabinateModel.id === 0) {
      let checkIndexNameDuplicate = false;
      this.fileCabinates.map(item => {
        if (item.indexName === this.fileCabinateModel.indexName) {
          checkIndexNameDuplicate = true;
        }
      });
      if (checkIndexNameDuplicate && errorList === '') {
        errorCount++;
        errorList += 'Index name already exists. Please enter unique name' + '<br>';
      }
    }

    if (errorCount > 0) {
      this.errorMessage.push({ severity: 'error', detail: errorList });
      return;
    }

    if (this.fileCabinateModel.id !== 0) { /* check new case or an update case */
      this.fileCabinates.splice(this.fileCabinateModel.id - 1, 1);
      this.fileCabinates.splice(this.fileCabinateModel.id - 1, 0, this.fileCabinateModel);
    } else {
      this.fileCabinateModel.id = this.fileCabinates.length + 1;
      this.fileCabinates.splice(this.fileCabinates.length, 0, this.fileCabinateModel);
    }

    this.fileCabinateModel = new FileCabinateModel();
    this.list = '';
  }


  /* save file cabinates */
  private saveFileCabinate() {
    this.fileCabinateService.saveFileCabinate(this.fileCabinates).then(res => {
      this.errorMessage.push({ severity: 'success', summary: 'Success', detail: 'Result Saved' });
    });
  }

  /* edit selected file cabinate */
  private editFileCabinate(selectedFileCabinate) {
    this.fileCabinateModel.id = selectedFileCabinate.id;
    this.fileCabinateModel.indexName = selectedFileCabinate.indexName;
    this.fileCabinateModel.defaultValue = selectedFileCabinate.defaultValue;
    this.fileCabinateModel.listValue = selectedFileCabinate.listValues;
    this.fileCabinateModel.indexType = selectedFileCabinate.indexType;
  }

  /* deleted selected file cabinate */
  private deleteFileCabinate(selectedFileCabinate) {
    this.fileCabinates.splice(selectedFileCabinate.id - 1, 1);
  }

}
