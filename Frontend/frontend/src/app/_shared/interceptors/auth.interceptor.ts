import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { EnvironmentService } from '../services/environment.service';
import { TokenService } from '../services/auth-token.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private environmentService: EnvironmentService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const token = TokenService.GetTokenString();
    const isAPI = request.url.startsWith(this.environmentService.urlAPI);

    if (token && isAPI) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(request);
  }
}

export const AuthInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
];
