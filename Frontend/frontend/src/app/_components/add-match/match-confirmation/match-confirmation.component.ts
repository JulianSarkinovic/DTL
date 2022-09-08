import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { IMatchFull } from '@entities/response/iMatchFull';
import { ConfirmationStatus } from '@enums/confirmation-status.enum';
import { TokenService } from '@services/auth-token.service';
import { MatchService } from '@services/match.service';
import { NotifierWrapService } from '@shared/services/notifier-wrap.service';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { MatchConfirmationDialogComponent } from './match-confirmation-dialog/match-confirmation-dialog.component';

@Component({
  selector: 'app-match-confirmation',
  templateUrl: './match-confirmation.component.html',
  styleUrls: ['./match-confirmation.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchConfirmationComponent implements OnInit {
  token?: string;
  matchId?: number;
  match$?: Observable<IMatchFull | undefined>;

  isSubmitting$ = this.matchService.isSubmitting$;

  constructor(
    private route: ActivatedRoute,
    private matchService: MatchService,
    private notifier: NotifierWrapService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.matchId = +(params.matchId as string);
      this.token = params.token as string;
      this.match$ = this.matchService.getMatchFull(this.matchId).pipe(
        tap((match) => {
          if (!match) {
            this.notifier.notify(
              "I can't find the match you're trying to view, sorry.",
              'red'
            );
            return;
          }
        })
      );
    });
  }

  onConfirm(agreed: boolean): void {
    if (!this.token || !this.matchId) {
      this.notifier.notify(
        'Something went wrong. Did you edit the link after clicking it?'
      );
      return;
    }

    this.matchService.confirmMatch({
      token: this.token,
      matchId: this.matchId,
      agreed: agreed,
    });
  }

  onEdit(match: IMatchFull): void {
    this.navigateIfNotAwaiting(match.confirmationStatus);

    const dialogRef = this.dialog.open(MatchConfirmationDialogComponent, {
      width: '275px',
      data: this.getOpponentName(match),
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === false) {
        this.onConfirm(false);
      }
      if (result === true) {
        void this.router.navigate(['/add-match'], { state: { match: match } });
      }
    });
  }

  private getOpponentName(match: IMatchFull): string | undefined {
    const user = TokenService.GetUser();

    if (user && user.id === match.playerOneId) {
      return match.playerTwoFirstName;
    } else if (user && user.id === match.playerTwoId) {
      return match.playerOneFirstName;
    } else {
      return undefined;
    }
  }

  private navigateIfNotAwaiting(confirmationStatus: ConfirmationStatus) {
    let route: string;
    if (confirmationStatus === ConfirmationStatus.Confirmed) {
      route = 'already-confirmed';
    }
    if (confirmationStatus === ConfirmationStatus.Declined) {
      route = 'already-declined';
    } else {
      return;
    }

    void this.router.navigate(['match-confirmed'], {
      queryParams: { result: route },
    });
  }
}
