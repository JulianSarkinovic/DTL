import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '@services/account.service';

@Component({
  templateUrl: './password-forgot.component.html',
  styleUrls: ['./password-forgot.component.scss'],
})
export class PasswordForgotComponent {
  constructor(
    private accountService: AccountService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe((params) => {
      this.email = params.email as string;
    });
  }

  email?: string;
  isSubmitting$ = this.accountService.isSubmitting$;
  isSubmitted = false;

  onReset(email: string): void {
    this.accountService.resetPassword(email);
    this.isSubmitted = true;
  }
}
