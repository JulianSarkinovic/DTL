import { Injectable } from '@angular/core';
import { RepositoryService } from '@services/repository.service';
import { RepositoryBaseService } from '@services/repository-base.service';
import { ErrorHandlerService } from '@services/error-handler.service';
import { IMatch } from '@entities/response/iMatch';
import { Match } from '@entities/request/match';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { Router } from '@angular/router';
import { IApiResponse, IApiResponseEmpty } from '@interfaces/iApiResponse';
import { catchError, delay, map, switchMap } from 'rxjs/operators';
import { BehaviorSubject, EMPTY, Observable, of } from 'rxjs';
import { IMatchFull } from '@entities/response/iMatchFull';

@Injectable({
  providedIn: 'root',
})
export class MatchService extends RepositoryBaseService<IMatch, Match> {
  constructor(
    protected repositoryService: RepositoryService,
    protected errorHandler: ErrorHandlerService,
    protected notifier: NotifierWrapService,
    router: Router
  ) {
    super(repositoryService, errorHandler, notifier, router);
  }

  protected isSubmittingSubject = new BehaviorSubject<boolean>(false);
  isSubmitting$ = this.isSubmittingSubject.asObservable().pipe(
    switchMap((isSubmitting) => {
      return isSubmitting ? of(true).pipe(delay(1000)) : of(false);
    })
  );

  public getMatchFull(matchId: number): Observable<IMatchFull | undefined> {
    this.isLoadingSubject.next(true);

    return this.repositoryService
      .get<IApiResponse<IMatchFull>>(
        this.getRoute({ method: 'getFull', id: matchId })
      )
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
        })
      );
  }

  public confirmMatch(matchConfirmationDto: {
    token: string;
    matchId: number;
    agreed: boolean;
  }): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponseEmpty>(
        this.getRoute({ method: 'confirmMatch' }),
        matchConfirmationDto
      )
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          void this.router.navigate(['match-confirmed'], {
            queryParams: { result: data.route },
          });
        },
        error: () => {
          this.isSubmittingSubject.next(false);
          this.notifier.notify(
            'Nothing happened, because something unexpectedly went wrong.',
            'red'
          );
        },
      });
  }

  protected getTypeName(): string {
    return 'match';
  }

  protected getObjectName(): string {
    return `Match`;
  }
}
