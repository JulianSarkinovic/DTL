import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appConfirmPassword]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useClass: ConfirmPasswordValidatorDirective,
      multi: true,
    },
  ],
})
export class ConfirmPasswordValidatorDirective implements Validator {
  validate(c: FormControl): { passwordMatchError: boolean } | null {
    const Password = c.parent?.get('password');
    const ConfirmPassword = c;

    if (ConfirmPassword === null) return null;
    if (Password) {
      const subscription: Subscription = Password.valueChanges.subscribe(() => {
        ConfirmPassword.updateValueAndValidity();
        subscription.unsubscribe();
      });
    }

    return Password && Password.value !== ConfirmPassword.value
      ? { passwordMatchError: true }
      : null;
  }
}
