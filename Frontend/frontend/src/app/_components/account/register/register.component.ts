import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IPlayerRegister } from '@entities/request/iPlayerRegister';
import { AccountService } from 'src/app/_shared/services/account.service';

@Component({
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {
  constructor(private accountService: AccountService) {}
  loginFailedErrorMessage$ = this.accountService.loginFailedErrorMessage$;
  isSubmitting$ = this.accountService.isSubmitting$;
  passwordHidden = true;

  onTogglePasswordType(): void {
    this.passwordHidden = !this.passwordHidden;
  }

  onRegister(form: NgForm): void {
    this.accountService.register(form.value as IPlayerRegister);
  }
}
