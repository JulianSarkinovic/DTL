import { Injectable } from '@angular/core';
import { RepositoryService } from '@services/repository.service';
import { ErrorHandlerService } from '@services/error-handler.service';
import { Club } from '@entities/request/club';
import { RepositoryBaseService } from '@services/repository-base.service';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { Router } from '@angular/router';
import { IClub } from '@entities/response/iClub';

@Injectable({
  providedIn: 'root',
})
export class ClubService extends RepositoryBaseService<IClub, Club> {
  constructor(
    protected repositoryService: RepositoryService,
    protected errorHandler: ErrorHandlerService,
    protected notifier: NotifierWrapService,
    router: Router
  ) {
    super(repositoryService, errorHandler, notifier, router);
  }

  protected getTypeName(): string {
    return 'club';
  }
  protected getObjectName(club: IClub): string {
    return `Club ${club.name}`;
  }

  search(name?: string): void {
    const route = this.getRoute({
      method: 'search',
      queryParams: [`name=${name || 'undefined'}`],
    });
    this.searchTermSubject.next(route);
  }
}
