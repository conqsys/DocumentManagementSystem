import { Component, OnInit } from '@angular/core';
import { ImportModel } from './shared/import.model';
import { Message } from 'primeng/primeng';
// import { ImportInfoModel } from './shared/import.model';
import { FileCabinateService } from '../admin/file-cabinet/shared/file-cabinate.service';

@Component({
  selector: 'sp-import',
  templateUrl: './import.component.html',
})

export class ImportComponent implements OnInit {

  private errorMsg: Message[] = [];
  private setDefault: string = '';
  private uploadedFiles: Array<any> = [];
  private importDetail: ImportModel = new ImportModel();
  constructor(private fileCabinateService: FileCabinateService) {

  }

  ngOnInit() {

    this.getFileCabinates();
  }

  private onUpload(image) {

    let FR = new FileReader();
    FR.onload = (e) => {
      console.log('image file');
      console.log(image.target.files[0].name);
      console.log((e.target as any).result);
      this.importDetail.imageName = image.target.files[0].name;
      this.importDetail.imageData = (e.target as any).result;

    };
    FR.readAsDataURL(image.target.files[0]);


    // for (let file of image.files) {
    //   this.uploadedFiles.push(file);
    // }

    // let imageUrl: any;
    // let imageObject = image.files[0];
    // // this.userDetail.imageName = imageObject.name;
    // // this.userDetail.imageType = imageObject.type;
    // let reader = new FileReader();
    // reader.readAsDataURL(imageObject);
    // reader.onload = function () {
    //   imageUrl = reader.result;
    // };

    // //   setTimeout(() => {
    // //     this.userDetail.imageSource = imageUrl;
    // //   }, 10);

    // this.errorMsg = [];
    // this.errorMsg.push({ severity: 'info', summary: 'File Uploaded', detail: '' });
  }

  private saveImport() {
    console.log(this.importDetail);

  }

  public getFileCabinates() {
    this.fileCabinateService.getFileCabinates().then(result => {
      result.data.map(res => {
        if (res.indexType === 1) {
          this.setDefault = res.defaultValue;
          let listOfValue = JSON.parse(res.listValue);
          res.listValue = [];
          listOfValue.map(lof => {
            let lofDetail = { label: lof, value: lof };
            res.listValue.push(lofDetail);
          });
        }
      });

      this.importDetail.listValue = result.data;
      // console.log(this.fileCabinates);
    });
  }

}
