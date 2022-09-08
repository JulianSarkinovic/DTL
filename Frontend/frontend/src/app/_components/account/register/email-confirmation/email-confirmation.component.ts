import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '@services/account.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.scss'],
})
export class EmailConfirmationComponent implements OnInit {
  isSubmitting$ = this.accountService.isSubmitting$;
  userId?: string;
  token?: string;

  constructor(
    private route: ActivatedRoute,
    private accountService: AccountService
  ) {}

  ngOnInit(): void {
    // Todo: send request to see if email is already confirmed and if so, reroute.

    this.route.queryParams.subscribe((params) => {
      this.userId = params.userId as string;
      this.token = params.token as string;
    });
  }

  onConfirm(): void {
    const emailConfirmedDto = {
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      userId: this.userId!,
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      token: this.token!,
    };

    this.accountService.confirmEmail(emailConfirmedDto);
  }
}
