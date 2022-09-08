import { Injectable } from '@angular/core';
import { RepositoryService } from '@services/repository.service';
import { ErrorHandlerService } from '@services/error-handler.service';
import { IPlayer } from '@entities/response/iPlayer';
import { IPlayerRegister } from '@entities/request/iPlayerRegister';
import { RepositoryBaseService } from '@services/repository-base.service';
import { IPlayerCreationModel } from '@entities/request/iPlayerCreationModel';
import { IApiResponse } from '@interfaces/iApiResponse';
import { BehaviorSubject, EMPTY, Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { Router } from '@angular/router';
import { IPlayerForProfile } from '@entities/response/iPlayerForProfile';

@Injectable({
  providedIn: 'root',
})
export class PlayerService extends RepositoryBaseService<
  IPlayer,
  IPlayerRegister
> {
  constructor(
    repositoryService: RepositoryService,
    errorHandler: ErrorHandlerService,
    notifier: NotifierWrapService,
    router: Router
  ) {
    super(repositoryService, errorHandler, notifier, router);
  }

  private isSubmittingSubject = new BehaviorSubject<boolean>(false);
  isSubmitting$ = this.isSubmittingSubject.asObservable();

  protected getTypeName(): string {
    return 'Player';
  }

  protected getObjectName(player: IPlayer): string {
    return `Player ${player.firstName} ${player.lastName}`;
  }

  getForProfile(id: string): Observable<IPlayerForProfile | undefined> {
    this.isLoadingSubject.next(true);

    return this.repositoryService
      .get<IApiResponse<IPlayerForProfile>>(
        this.getRoute({ method: 'getForProfile', id })
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

  findOrCreate(
    playerCreationModel: IPlayerCreationModel
  ): Observable<IPlayer | undefined> {
    this.isSubmittingSubject.next(true);

    return this.repositoryService
      .post<IApiResponse<IPlayer>>(
        this.getRoute({ method: 'findOrCreate' }),
        playerCreationModel
      )
      .pipe(
        map((data) => {
          this.isSubmittingSubject.next(false);
          if (data.success) return data.resultData;
          this.errorHandler.handleError(data.errorMessage);
          return;
        }),
        catchError((err) => {
          this.isSubmittingSubject.next(false);
          this.errorHandler.handleError(err);
          return EMPTY;
        })
      );
  }

  search(fullName?: string): void {
    const route = this.getRoute({
      method: 'search',
      queryParams: [`fullName=${fullName || 'undefined'}`],
    });
    this.searchTermSubject.next(route);
  }
}
