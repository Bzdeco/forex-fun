import { HttpInterceptor, HttpRequest, HttpHandler, HttpSentEvent, HttpHeaderResponse, HttpProgressEvent, HttpUserEvent, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/do';
import { HttpResponse } from "selenium-webdriver/http";
import { Router } from "@angular/router";
import { Injectable } from "@angular/core";

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.headers.get('No-Auth') == 'True')
      return next.handle(req.clone());

    if (localStorage.getItem('token') != null) {
      const clonedReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
      });
      return next.handle(clonedReq).do(
        succ => { },
        err => {
          if (err.status == 401) {
            console.log('Unauthorized access');
            this.router.navigate(['/']);
          }
        }
      );
    }
    else {
      console.log('Token missing');
      this.router.navigate(['/']);
    }
  }
}
