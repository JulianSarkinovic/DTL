import { Injectable } from '@angular/core';
import { BehaviorSubject, EMPTY, Observable, Subject } from 'rxjs';
import { merge, combineLatest, of } from 'rxjs';
import {
  map,
  tap,
  catchError,
  scan,
  debounceTime,
  shareReplay,
} from 'rxjs/operators';
import { distinctUntilChanged, switchMap, delay } from 'rxjs/operators';
import { RepositoryService } from '@services/repository.service';
import { ErrorHandlerService } from '@services/error-handler.service';
import { IApiResponse } from '@interfaces/iApiResponse';
import { IRemovable } from '@interfaces/iRemovable';
import { Status } from '@enums/status.enum';
import { IResponseBase } from '@entities/response/iResponseBase';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { Router } from '@angular/router';
import { IRouteParameters } from '@interfaces/iRouteParameters';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export abstract class RepositoryBaseService<
  TResponse extends IResponseBase<number | string>,
  TRequest
> {
  protected isLoadingSubject = new BehaviorSubject<boolean>(false);
  isLoading$ = this.isLoadingSubject.asObservable().pipe(
    switchMap((loading) => {
      return loading ? of(true).pipe(delay(1000)) : of(false);
    })
  );

  protected isSearchingSubject = new BehaviorSubject<boolean>(false);
  isSearching$ = this.isSearchingSubject.asObservable().pipe(
    switchMap((loading) => {
      return loading ? of(true).pipe(delay(1000)) : of(false);
    })
  );

  protected isAddingSubject = new BehaviorSubject<boolean>(false);
  isAdding$ = this.isAddingSubject.asObservable();

  // 1
  // Getting the objects from the server;
  private objectsRaw$: Observable<TResponse[] | undefined> =
    this.repositoryService
      .get<IApiResponse<TResponse[]>>(this.getRoute({ method: 'get' }))
      .pipe(
        map((data) => {
          this.isLoadingSubject.next(false);
          if (data.success && data.resultData) return data.resultData;
          this.errorHandler.handleError(data.errorMessage);
          return;
        }),
        catchError((err) => {
          this.isLoadingSubject.next(false);
          this.errorHandler.handleError(err);
          return EMPTY;
        }),
        shareReplay() // Todo, needs a refresh possibility when e.g. something is added, by ANYONE.
      );

  // 2
  // Updating the stream on add;
  protected addSubject = new Subject<TResponse>();
  protected add$ = this.addSubject.asObservable();
  private objectsWithAdd$ = merge(this.objectsRaw$, this.add$).pipe(
    scan((acc, value) => [...(acc as TResponse[]), value] as TResponse[])
  );

  // 3
  // Updating the stream on delete:
  protected deleteSubject = new BehaviorSubject<number | string>(-1);
  private objectsWithAddDelete$ = combineLatest([
    this.objectsWithAdd$ as Observable<TResponse[]>,
    this.deleteSubject.asObservable(),
  ]).pipe(
    map(([objects, deleteId]) => {
      const index = objects.findIndex((object) => object.id === deleteId);
      if (index >= 0) {
        objects.splice(index, 1);
      }
      return objects;
    })
  );

  // 4
  // Updating the stream on update;
  protected updateSubject = new BehaviorSubject<TResponse | null>(null);
  private objectsWithAddDeleteUpdate = combineLatest([
    this.objectsWithAddDelete$,
    this.updateSubject.asObservable(),
  ]).pipe(
    map(([objects, updateObject]) => {
      if (!updateObject) return objects;
      const index = objects.findIndex(
        (object) => object.id === updateObject.id
      );
      if (index >= 0) {
        objects[index] = updateObject;
      }
      return objects;
    })
  );

  // 5
  // Searching the back-end;
  protected searchTermSubject = new Subject<string>();
  private objectsSearched$ = this.searchTermSubject.pipe(
    debounceTime(500),
    tap(() => this.isSearchingSubject.next(true)),
    distinctUntilChanged(),
    switchMap((route) =>
      this.repositoryService.get<IApiResponse<TResponse[]>>(route)
    ),
    map((data) => {
      this.isSearchingSubject.next(false);
      if (data.success && data.resultData) return data.resultData;
      this.errorHandler.handleError(data.errorMessage);
      return;
    }),
    catchError((err) => {
      this.isSearchingSubject.next(false);
      this.errorHandler.handleError(err);
      return EMPTY;
    })
  );

  // 6
  // Merging searched observable with stream;
  data$ = merge(this.objectsWithAddDeleteUpdate, this.objectsSearched$).pipe(
    scan((acc, value) => value)
  );

  constructor(
    protected repositoryService: RepositoryService,
    protected errorHandler: ErrorHandlerService,
    protected notifier: NotifierWrapService,
    protected router: Router
  ) {}

  protected abstract getTypeName(): string;
  protected abstract getObjectName(object: TResponse): string;

  add(
    object: TRequest,
    routeOnSuccess?: string,
    messageOnSucces: string = 'added'
  ): Observable<TResponse> {
    this.isAddingSubject.next(true);
    this.repositoryService
      .post<IApiResponse<TResponse>>(this.getRoute({ method: 'post' }), object)
      .subscribe({
        next: (data) => {
          this.isAddingSubject.next(false);
          if (data.success && data.resultData) {
            this.addSubject.next(data.resultData);
            messageOnSucces === 'added'
              ? this.notify(data.resultData, 'added')
              : this.notifier.notify(messageOnSucces, 'green');
            if (routeOnSuccess) this.redirect(routeOnSuccess);
          } else {
            this.errorHandler.handleError(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.isAddingSubject.next(false);
          this.errorHandler.handleError(err);
        },
      });

    return this.add$;
  }

  get(
    id: number | string,
    method: string = 'get'
  ): Observable<TResponse | undefined> {
    this.isLoadingSubject.next(true);

    return this.repositoryService
      .get<IApiResponse<TResponse>>(this.getRoute({ method, id }))
      .pipe(
        map((data) => {
          this.isLoadingSubject.next(false);
          if (data.success) return data.resultData;
          this.errorHandler.handleError(data.errorMessage);
          return;
        }),
        catchError((err) => {
          this.isLoadingSubject.next(false);
          this.errorHandler.handleError(err);
          return EMPTY;
        })
      );
  }

  update(
    object: TResponse | IRemovable<string | number>,
    action = 'updated'
  ): void {
    this.repositoryService
      .put<IApiResponse<TResponse>>(
        this.getRoute({ method: 'put', id: object.id }),
        object
      )
      .subscribe({
        next: (data) => {
          if (data.success && data.resultData) {
            this.updateSubject.next(data.resultData);
            this.notify(data.resultData, action);
          } else {
            this.errorHandler.handleError(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => this.errorHandler.handleError(err),
      });
  }

  delete(object: TResponse): void {
    this.repositoryService
      .delete(this.getRoute({ method: 'delete', id: object.id }))
      .subscribe({
        next: () => {
          if (object.id === undefined) {
            // Todo: this should be handled better.
            return;
          }
          this.deleteSubject.next(object.id);
          this.notify(object, 'deleted');
        },
        error: (err: HttpErrorResponse) => this.errorHandler.handleError(err),
      });
  }

  disable(object: IRemovable<string | number>): void {
    object.status = Status.Disabled;
    this.update(object, 'disabled');
  }

  enable(object: IRemovable<string | number>): void {
    object.status = Status.Enabled;
    this.update(object, 'enabled');
  }

  public getRoute(params?: IRouteParameters): string {
    let route = this.getTypeName();
    if (!params) return route;
    if (params.method) route += `/${params.method}`;
    if (params.id) route += `/${params.id}`;
    if (params.queryParams) {
      route += '?';
      let first = true;
      for (const queryParam of params.queryParams) {
        if (queryParam.includes('undefined')) continue;
        if (queryParam.includes('null')) continue;
        if (!first) route += '&';
        first = false;
        route += queryParam;
      }
    }
    return route;
  }

  private redirect(route: string): void {
    void this.router.navigate([route]);
  }

  private notify(object: TResponse, action: string): void {
    this.notifier.notify(
      `The ${this.getObjectName(object)} has been ${action} succesfully.`,
      'green'
    );
  }
}
