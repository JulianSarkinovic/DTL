/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
/* eslint-disable @typescript-eslint/no-explicit-any */

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EnvironmentService } from '@services/environment.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RepositoryService {
  constructor(
    private http: HttpClient,
    private environment: EnvironmentService
  ) {}

  get<T>(route: string): Observable<T> {
    return this.http.get<T>(this.getCompleteRoute(route));
  }

  post<T>(route: string, body: any): Observable<T> {
    return this.http.post<T>(this.getCompleteRoute(route), body);
  }

  put<T>(route: string, body: any): Observable<T> {
    return this.http.put<T>(this.getCompleteRoute(route), body);
  }

  delete(route: string): Observable<void> {
    return this.http.delete<void>(this.getCompleteRoute(route));
  }

  private getCompleteRoute(route: string): string {
    return `${this.environment.urlAPI}/${route}`;
  }
}
