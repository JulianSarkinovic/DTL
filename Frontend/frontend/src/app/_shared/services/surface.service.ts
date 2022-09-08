import { Injectable } from '@angular/core';
import { RepositoryService } from '@services/repository.service';
import { Observable, EMPTY, combineLatest, BehaviorSubject } from 'rxjs';
import { map, catchError, shareReplay } from 'rxjs/operators';
import { IApiResponse } from '@interfaces/iApiResponse';
import { ErrorHandlerService } from '@services/error-handler.service';
import { ISurface } from '@entities/response/iSurface';

@Injectable({
  providedIn: 'root',
})
export class SurfaceService {
  constructor(
    private repositoryService: RepositoryService,
    private errorHandler: ErrorHandlerService
  ) {}

  // 1
  // Getting the surfaces from the server;
  private surfacesRaw$: Observable<ISurface[] | undefined> =
    this.repositoryService.get<IApiResponse<ISurface[]>>('surface/get').pipe(
      map((data) => {
        if (data.success) return data.resultData;
        this.errorHandler.handleError(data.errorMessage);
        return;
      }),
      catchError((err) => {
        this.errorHandler.handleError(err);
        return EMPTY;
      }),
      shareReplay(1)
    );

  // 6
  // Filtering the stream (searching the front-end);
  private filterSubject = new BehaviorSubject<string>('');
  surfaces$ = combineLatest([
    this.surfacesRaw$,
    this.filterSubject.asObservable(),
  ]).pipe(
    map(([surfaces, filter]) =>
      surfaces?.filter((surface) =>
        filter
          ? surface.name.toLowerCase().includes(filter.toLowerCase())
          : true
      )
    )
  );

  filter(term?: string): void {
    this.filterSubject.next(term || '');
  }
}
