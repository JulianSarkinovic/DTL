import { Injectable } from '@angular/core';
import { RepositoryService } from './repository.service';
import { ErrorHandlerService } from './error-handler.service';
import { Router } from '@angular/router';
import { IApiResponseEmpty, IApiResponse } from '@interfaces/iApiResponse';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { IPlayerChangePasswordRequest } from '../interfaces/iPlayerChangePasswordRequest';
import { TokenService } from '@services/auth-token.service';
import { IPlayer } from '@entities/response/iPlayer';
import { IPlayerLogin } from '@interfaces/iPlayerLogin';
import { IPlayerRegister } from '@entities/request/iPlayerRegister';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PlayerResetPasswordModel } from '@entities/request/playerResetPasswordModel';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(
    private notifier: NotifierWrapService,
    private repositoryService: RepositoryService,
    private tokenService: TokenService,
    private errorHandler: ErrorHandlerService,
    private router: Router
  ) {
    this.initFromStorage();
  }

  private userSubject = new BehaviorSubject<IPlayer | null>(null);
  user$: Observable<IPlayer | null> = this.userSubject.asObservable();

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$: Observable<boolean> =
    this.isAuthenticatedSubject.asObservable();

  private loginFailedErrorMessageSubject = new Subject<string | undefined>();
  loginFailedErrorMessage$: Observable<string | undefined> =
    this.loginFailedErrorMessageSubject.asObservable();

  private isSubmittingSubject = new BehaviorSubject<boolean>(false);
  isSubmitting$ = this.isSubmittingSubject.asObservable();

  login(body: IPlayerLogin): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponse<IPlayer>>('account/authenticate', body)
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          if (data.success && data.resultData) {
            this.processUser(data.resultData);
          } else {
            this.loginFailedErrorMessageSubject.next(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.isSubmittingSubject.next(false);
          err.status === 0
            ? this.errorHandler.handleError(err)
            : this.loginFailedErrorMessageSubject.next(
                'Something went wrong on the server during log in.'
              );
        },
      });
  }

  logout(): void {
    TokenService.RemoveUserAndToken();
    this.isAuthenticatedSubject.next(false);
    this.userSubject.next(null);
    void this.router.navigate(['']);
  }

  register(user: IPlayerRegister): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponse<IPlayer>>('account/register', user)
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          if (data.success) {
            this.processUser(data.resultData);
            this.notifier.notify(
              "Welcome to DTL! You'll receive a confirmation email shortly.",
              'green'
            );
          } else {
            this.loginFailedErrorMessageSubject.next(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.isSubmittingSubject.next(false);
          err.status === 0
            ? this.errorHandler.handleError(err)
            : this.loginFailedErrorMessageSubject.next(
                'Something went wrong on the server during sign up.'
              );
        },
      });
  }

  confirmEmail(emailConfirmationModel: {
    userId: string;
    token: string;
  }): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponse<string>>(
        'account/registerConfirm',
        emailConfirmationModel
      )
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          void this.router.navigate(['email-confirmed'], {
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

  changePassword(changePasswordDto: IPlayerChangePasswordRequest): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponseEmpty>('account/changePassword', changePasswordDto)
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          if (!data.success) {
            this.loginFailedErrorMessageSubject.next(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.isSubmittingSubject.next(false);
          this.errorHandler.handleError(err);
        },
      });
  }

  resetPassword(email: string): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponseEmpty>('account/resetPassword', { email: email })
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          if (!data.success) {
            this.loginFailedErrorMessageSubject.next(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.isSubmittingSubject.next(false);
          this.errorHandler.handleError(err);
        },
      });
  }

  resetPasswordConfirm(resetPasswordDto: PlayerResetPasswordModel): void {
    this.isSubmittingSubject.next(true);
    this.repositoryService
      .post<IApiResponse<IPlayer>>(
        'account/resetPasswordConfirm',
        resetPasswordDto
      )
      .subscribe({
        next: (data) => {
          this.isSubmittingSubject.next(false);
          if (data.success && data.resultData) {
            this.processUser(data.resultData);
          } else {
            this.loginFailedErrorMessageSubject.next(data.errorMessage);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.isSubmittingSubject.next(false);
          err.status === 0
            ? this.errorHandler.handleError(err)
            : this.loginFailedErrorMessageSubject.next(
                'Something went wrong on the server during password reset.'
              );
        },
      });
  }

  private initFromStorage(): void {
    if (!this.tokenService.isAuthenticated()) return;
    this.userSubject.next(TokenService.GetUser());
    this.isAuthenticatedSubject.next(true);
  }

  private processUser(user: IPlayer | undefined): void {
    if (user === undefined || user.token === undefined) {
      // Todo: This should never be possible. Throw some error and/or log it.
      return;
    }

    TokenService.SetTokenString(user.token);
    TokenService.SetUser(user);

    this.loginFailedErrorMessageSubject.next(undefined);

    if (!this.tokenService.isAuthenticated()) {
      this.errorHandler.handleError(
        'The server provided an invalid token. Please contact the administrator.'
      );
      return;
    }

    this.userSubject.next(user);
    this.isAuthenticatedSubject.next(true);
    void this.router.navigate(['']);
  }
}
