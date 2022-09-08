import { Component, ChangeDetectionStrategy } from '@angular/core';
import { PlayerService } from '../../_shared/services/player.service';
import { AccountService } from '../../_shared/services/account.service';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { NotifierWrapService } from '@services/notifier-wrap.service';

@Component({
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RankingComponent {
  players$ = this.playersService.data$;
  displayedColumns = ['rank', 'player', 'points'];

  vm$ = combineLatest([
    this.players$,
    this.accountService.isAuthenticated$,
    this.accountService.user$,
  ]).pipe(
    map(([players, isAuthenticated, user]) => ({
      players,
      isAuthenticated,
      user,
    }))
  );

  private notifiedCount = 0;

  constructor(
    private playersService: PlayerService,
    private accountService: AccountService,
    private notifier: NotifierWrapService
  ) {}

  onCrown(): void {
    this.notifiedCount++;
    switch (this.notifiedCount) {
      case 1:
        this.notifier.notify('Hello');
        break;
      case 4:
        this.notifier.notify(`How you doing?`);
        break;
      default:
        break;
    }
  }
}
