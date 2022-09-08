import { Component } from '@angular/core';
import { AccountService } from 'src/app/_shared/services/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
  user$ = this.accountService.user$;

  constructor(private accountService: AccountService) {}

  onLogout(): void {
    this.accountService.logout();
  }
}
