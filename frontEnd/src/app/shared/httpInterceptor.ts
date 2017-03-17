import { EventEmitter } from '@angular/core';
import { Http, Request, RequestOptionsArgs, Response, RequestOptions, ConnectionBackend, Headers } from '@angular/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { LocalStorageService } from 'angular-2-local-storage';
import { PubSubService } from '../interceptor/pub-service';
import { BaseComponent } from '../base.component';


export class HttpInterceptor extends Http {
  _pubsub: PubSubService;
  authorization: any;
  requested: EventEmitter<string>;
  completed: EventEmitter<string>;
  error: EventEmitter<string>;

  constructor(backend: ConnectionBackend,
    defaultOptions: RequestOptions,
    private router: Router,
    private pubsub: PubSubService,
    private localStorageService: LocalStorageService
  ) {
    super(backend, defaultOptions);
    this.requested = new EventEmitter<string>();
    this.completed = new EventEmitter<string>();
    this.error = new EventEmitter<string>();

  }


  request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
    return super.request(url, options);
  }

  get(url: string, options?: RequestOptionsArgs): Observable<Response> {
    this.requestInterceptor();
    options = this.addHeaders(options);
    return super.get(url, options)
      .catch(this.onCatch)
      .do((res: Response) => {
        this.onSubscribeSuccess(res);
      }, (error: any) => {
        this.onSubscribeError(error);
      })
      .finally(() => {
        this.onFinally();
      });
  }


  post(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {

    this.requestInterceptor();
    return super.post(url, body, this.requestOptions(options))
      .catch(this.onCatch)
      .do((res: Response) => {
        this.onSubscribeSuccess(res);
      }, (error: any) => {
        this.onSubscribeError(error);
      })
      .finally(() => {
        this.onFinally();
      });
  }


  put(url: string, body: string, options?: RequestOptionsArgs): Observable<Response> {
    this.requestInterceptor();
    return super.put(url, body, this.requestOptions(options))
      .catch(this.onCatch)
      .do((res: Response) => {
        this.onSubscribeSuccess(res);
      }, (error: any) => {
        this.onSubscribeError(error);
      })
      .finally(() => {
        this.onFinally();
      });

  }

  delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
    this.requestInterceptor();
    return super.delete(url, options)
      .catch(this.onCatch)
      .do((res: Response) => {
        this.onSubscribeSuccess(res);
      }, (error: any) => {
        this.onSubscribeError(error);
      })
      .finally(() => {
        this.onFinally();
      });
  }


  private requestOptions(options?: RequestOptionsArgs): RequestOptionsArgs {
    let isHeaderFound = true;
    if (!options) {
      isHeaderFound = false;
    }
    options = this.addHeaders(options);
    if (!isHeaderFound) {
      options.headers.append('Content-Type', 'application/json');
    }
    return options;
  }

  onCatch(error: any, caught: Observable<any>): Observable<any> {
    return Observable.throw(error);
    // if (error.status === 401) {                  
    //   this.authorization = this.localStorageService.get('authorization');
    //   if (this.authorization) {
    //     this.localStorageService.remove('authorization');
    //     this.localStorageService.remove('user');
    //   }
    //   this.router.navigate(['/login']);
    //   return Observable.empty();

    // } else if (error.status === 403) {
    //   console.log('you can not access api');
    //   return Observable.throw(error);
    // } else if (error.status === 0) {                // Api Connection Refused
    //   console.log('ERR_CONNECTION_REFUSED, Api is down');
    //   return Observable.throw(error);
    // } else {
    //   return Observable.throw(error);
    // }
  }

  private onSubscribeSuccess(res: Response): void {
  }

  private onSubscribeError(error: any): void {
    this.authorization = this.localStorageService.get('authorization');
    if (error.status === 401) {
      if (this.authorization) {
        this.localStorageService.remove('authorization');
        this.localStorageService.remove('user');
      }
      this.router.navigate(['/login']);

    } else if (error.status === 403) {
      // console.log('you can not access api');
    } else if (error.status === 0) {                // Api Connection Refused
      // console.log('ERR_CONNECTION_REFUSED, Api is down');
    } else {
      if (this.authorization === null) {
        this.localStorageService.remove('sessionData');
        this.router.navigate(['/login']);
      }
    }
  }

  private onFinally(): void {
    this.responseInterceptor();
  }

  private responseInterceptor(): void {
    this.pubsub.afterRequest.emit("afterRequestEvent");
  }

  private requestInterceptor(): void {
    this.pubsub.beforeRequest.emit("beforeRequestEvent");
  }

  getRequestOptionArgs(options?: RequestOptionsArgs): RequestOptionsArgs {
    let isHeaderFound = true;
    if (!options) {
      isHeaderFound = false;
    }
    options = this.addHeaders(options);
    if (!isHeaderFound) {
      options.headers.append('Content-Type', 'application/json');
    }
    return options;
  }

  addHeaders(options?: RequestOptionsArgs): RequestOptionsArgs {
    if (options == null) {
      options = new RequestOptions();
    }

    if (options.headers == null) {
      options.headers = new Headers();
    }
    options.headers.delete('Cookie');
    // options.headers.delete('Cookie');
    // options.headers.append('Authorization', this.authorization);
    this.authorization = this.localStorageService.get('authorization');
    if (this.authorization) {
      options.headers.append('Authorization', this.authorization);
    }

    return options;
  }
}
