import { Component } from '@angular/core';
import { AccountService } from '@services/account.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss'],
})
export class FooterComponent {
  user$ = this.accountService.user$;

  constructor(private accountService: AccountService) {}
}
