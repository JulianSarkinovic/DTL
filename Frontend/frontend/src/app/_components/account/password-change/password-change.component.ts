import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IPlayerChangePasswordRequest } from '@interfaces/iPlayerChangePasswordRequest';
import { AccountService } from '@services/account.service';

@Component({
  selector: 'app-password-change',
  templateUrl: './password-change.component.html',
  styleUrls: ['./password-change.component.scss'],
})
export class PasswordChangeComponent {
  constructor(private accountService: AccountService) {}

  user$ = this.accountService.user$;
  loginFailedErrorMessage$ = this.accountService.loginFailedErrorMessage$;
  isSubmitting$ = this.accountService.isSubmitting$;
  passwordHidden = true;

  public onTogglePasswordType(): void {
    this.passwordHidden = !this.passwordHidden;
  }

  public onChange(form: NgForm): void {
    this.accountService.changePassword(
      form.value as IPlayerChangePasswordRequest
    );
  }
}
