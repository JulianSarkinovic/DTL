import { Component, ChangeDetectionStrategy } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IPlayerLogin } from '@interfaces/iPlayerLogin';
import { AccountService } from 'src/app/_shared/services/account.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  constructor(private accountService: AccountService) {}
  loginFailedErrorMessage$ = this.accountService.loginFailedErrorMessage$;
  isSubmitting$ = this.accountService.isSubmitting$;
  passwordHidden = true;

  public onTogglePasswordType(): void {
    this.passwordHidden = !this.passwordHidden;
  }

  public onLogin(form: NgForm): void {
    this.accountService.login(form.value as IPlayerLogin);
  }
}
