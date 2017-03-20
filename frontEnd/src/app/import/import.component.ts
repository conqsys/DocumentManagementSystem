import { Component, OnInit } from '@angular/core';
import { ImportInfoModel } from './shared/import.model';
import { Message } from 'primeng/primeng';
// import { ImportInfoModel } from './shared/import.model';

@Component({
  selector: 'sp-import',
  templateUrl: './import.component.html',
})

export class ImportComponent implements OnInit {

  private errorMsg: Message[] = [];

  private fileCabinates: Array<ImportInfoModel> = [
    { id: 1, indexName: 'client Code', defaultValue: 'test', listValues: '', indexType: 0, list: [] },
    { id: 2, indexName: 'Misc', defaultValue: 'test1', listValues: '["test1", "test2", "test3"]', indexType: 1, list: [] },
    { id: 3, indexName: 'Comments', defaultValue: 'test', listValues: '', indexType: 0, list: [] }
  ];
  private uploadedFiles: Array<any> = [];

  constructor() {

    this.fileCabinates.map(item => {
      if (item.listValues.length > 0) {
        let test = JSON.parse(item.listValues);
        test.map(list => {
          let demo = { label: list, value: list };
          item.list.push(demo);
        });
      }
    });

  }

  ngOnInit() { }

  private onUpload(image) {
    for (let file of image.files) {
      this.uploadedFiles.push(file);
    }

    let imageUrl: any;
    let imageObject = image.files[0];
    // this.userDetail.imageName = imageObject.name;
    // this.userDetail.imageType = imageObject.type;
    let reader = new FileReader();
    reader.readAsDataURL(imageObject);
    reader.onload = function () {
      imageUrl = reader.result;
    };

    //   setTimeout(() => {
    //     this.userDetail.imageSource = imageUrl;
    //   }, 10);

    this.errorMsg = [];
    this.errorMsg.push({ severity: 'info', summary: 'File Uploaded', detail: '' });
  }

  private saveImport() {

  }

}
