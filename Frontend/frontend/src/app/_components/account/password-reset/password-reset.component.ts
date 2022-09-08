/* eslint-disable @typescript-eslint/no-unsafe-member-access */
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PlayerResetPasswordModel } from '@entities/request/playerResetPasswordModel';
import { AccountService } from '@services/account.service';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss'],
})
export class PasswordResetComponent {
  constructor(
    private accountService: AccountService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe((params) => {
      this.token = params.token as string;
    });
  }

  token?: string;
  passwordHidden = true;
  loginFailedErrorMessage$ = this.accountService.loginFailedErrorMessage$;
  isSubmitting$ = this.accountService.isSubmitting$;

  public onTogglePasswordType(): void {
    this.passwordHidden = !this.passwordHidden;
  }

  public onReset(form: NgForm): void {
    const model = new PlayerResetPasswordModel(
      form.value.email as string,
      this.token as string,
      form.value.newPassword as string,
      form.value.confirmationPassword as string
    );

    this.accountService.resetPasswordConfirm(model);
  }
}
