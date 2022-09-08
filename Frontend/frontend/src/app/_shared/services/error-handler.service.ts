import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { NotifierWrapService } from '@services/notifier-wrap.service';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandlerService {
  constructor(private router: Router, private notifier: NotifierWrapService) {}

  public handleError(error: unknown): void {
    if (error instanceof HttpErrorResponse) {
      this.handleHttpError(error);
      // Todo: see if this is actually right, and if it is, then correct calls and catches to HttpErrorResponse | ErrorEvent | string
    } else if (error instanceof ErrorEvent) {
      const message = `An error occurred.`;
      this.showError(message);
    } else if (typeof error === 'string') {
      const message = error;
      this.showError(message);
    } else {
      const message = 'There was an unexpected error';
      this.showError(message);
    }
  }

  private handleHttpError(error: HttpErrorResponse): void {
    switch (error.status) {
      case 0:
        this.showError(
          'There was a problem processing your request, the server may be offline. Please try again later.'
        );
        break;
      case 401:
        this.showError(
          'Unauthorized?! Which is strange, because getting here means that something else went wrong. You need to be logged in to do this.'
        );
        break;
      case 403:
        this.showError(
          "Forbidden! You're not allowed to do this. If you are, let the admin know."
        );
        break;
      case 404: {
        void this.router.navigate(['/404']);
        break;
      }
      case 500:
        this.showError(
          'There was a problem processing your request. Please try again later.'
        );
        break;
      default: {
        const message = error.message
          ? error.message
          : 'There was an unexpected HTTP error.';
        this.showError(message);
        break;
      }
    }
  }

  private showError(message: string): void {
    this.notifier.notify(message, 'red');
  }
}
