import { ChangeDetectionStrategy, Component, ViewChild } from '@angular/core';
import { Set } from '@entities/request/set';
import { MatchService } from '@services/match.service';
import { IPlayer } from '@entities/response/iPlayer';
import { Match } from '@entities/request/match';
import { MatchType } from '@enums/match-type.enum';
import { Duration } from '@entities/request/duration';
import { MatchWinner } from '@enums/match-winner.enum';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Step } from '@enums/step.enum';
import { AddByEmailComponent } from '@components/add-match/match-opponent/add-by-email/add-by-email.component';
import { AccountService } from '@services/account.service';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { ISurface } from '@entities/response/iSurface';
import { IClub } from '@entities/response/iClub';
import { IMatchFull } from '@entities/response/iMatchFull';
import { IPlayerNameAndId } from '@entities/request/iPlayerNameAndId';
import { TokenService } from '@services/auth-token.service';
import { Router } from '@angular/router';

@Component({
  templateUrl: './add-match.component.html',
  styleUrls: ['./add-match.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddMatchComponent {
  @ViewChild(AddByEmailComponent)
  addByEmailComponent?: AddByEmailComponent;

  Step = Step;
  private activePageSubject = new BehaviorSubject<number>(1);
  activePage$ = this.activePageSubject.asObservable();

  mustValidateSetsSubject = new Subject<number>();
  user$: Observable<IPlayer | null> = this.accountService.user$;
  isAdding$: Observable<boolean> = this.matchService.isAdding$;

  matchId?: number;
  opponent?: IPlayerNameAndId;
  sets: Set[] = [];
  winner?: MatchWinner;
  club?: IClub;
  surface?: ISurface;
  duration?: Duration;
  ranked = true;
  match?: Match;

  constructor(
    private matchService: MatchService,
    private accountService: AccountService,
    private notifier: NotifierWrapService,
    private router: Router
  ) {
    const match = this.router.getCurrentNavigation()?.extras?.state
      ?.match as unknown;

    if (match) {
      this.onSetProperties(match as IMatchFull);
    }
  }

  private onSetProperties(match: IMatchFull): void {
    const user = TokenService.GetUser();

    if (user && user.id === match.playerOneId) {
      this.opponent = {
        id: match.playerTwoId,
        firstName: match.playerTwoFirstName,
        lastName: match.playerTwoLastName,
      };
    } else if (user && user.id === match.playerTwoId) {
      this.opponent = {
        id: match.playerOneId,
        firstName: match.playerOneFirstName,
        lastName: match.playerOneLastName,
      };
    } else {
      this.notifier.notify(
        "It looks like you're trying to edit a match of which you were no part.",
        'red'
      );
      return;
    }

    this.matchId = match.id;
    this.sets = Set.CreateSets(match.sets);
    this.winner = match.winner;
    this.club = match.club;
    this.surface = match.surface;
    this.duration = Duration.Create(match.duration);
    this.ranked = match.ranked;
    this.activePageSubject.next(3);
  }

  onAddMatch(userId: string): void {
    if (this.isStepDisabled(Step.Next)) return;

    if (this.opponent === null || this.opponent === undefined) {
      this.notifier.notify(
        'Something went wrong. Please select your opponent on the first tab.',
        'red'
      );
      return;
    }
    if (this.sets === undefined) {
      this.notifier.notify(
        'Something went wrong. Please check if the score was entered correctly.',
        'red'
      );
      return;
    }
    if (this.winner === undefined) {
      this.notifier.notify(
        'Something went wrong. Please select the match winner underneath the score input.',
        'red'
      );
      return;
    }
    if (this.ranked === undefined) {
      this.notifier.notify(
        'Something went wrong. Please select whether this match was ranked or not first.',
        'red'
      );
      return;
    }
    if (this.duration === undefined) {
      this.notifier.notify(
        'Something went wrong. Please select the duration of the match first.',
        'red'
      );
      return;
    }
    if (this.surface === undefined) {
      this.notifier.notify(
        "Something went wrong. Please try selecting the surface you've played on, again.",
        'red'
      );
      return;
    }

    this.match = new Match(
      this.matchId,
      userId,
      this.opponent.id,
      this.sets,
      MatchType.Singles,
      this.winner,
      this.ranked,
      this.duration,
      this.surface?.id,
      undefined,
      undefined,
      this.club?.id
    );

    this.matchService.add(
      this.match,
      'ranking',
      'A match confirmation email has been sent to your opponent.'
    );
  }

  onStep(step: Step): void {
    if (this.isStepDisabled(step)) return;

    if (this.activePageSubject.value === 1) {
      step += 1;
    }

    if (this.activePageSubject.value === 2 && step === Step.Next) {
      this.addByEmailComponent?.form.ngSubmit.emit();
      // Todo Loading time to consider, but that may remain behind the scenes..
      //   when this opponent has value, continue... wait until isSubitting?
    }

    if (this.activePageSubject.value === 3 && step === Step.Next) {
      this.mustValidateSetsSubject.next(1);
    }

    if (this.activePageSubject.value === 3 && step === Step.Back) {
      step -= 1;
    }

    this.activePageSubject.next(this.activePageSubject.value + step);
  }

  isStepDisabled(step: Step): boolean {
    if (
      this.activePageSubject.value === 1 &&
      step === Step.Next &&
      !this.opponent
    ) {
      return true;
    }
    if (
      this.activePageSubject.value === 2 &&
      step === Step.Next &&
      !this.addByEmailComponent?.form.valid
    ) {
      return true;
    }
    if (
      this.activePageSubject.value === 3 &&
      step === Step.Next &&
      !(this.sets.length > 0)
    ) {
      return true;
    }
    if (
      this.activePageSubject.value === 4 &&
      step === Step.Next &&
      !this.surface
    ) {
      return true;
    }
    return false;
  }
}
