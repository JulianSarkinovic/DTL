import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  ChangeDetectionStrategy,
} from '@angular/core';
import { Set } from '@entities/request/set';
import { AccountService } from '@services/account.service';
import { MatchPlayer } from '@enums/match-player.enum';
import { MatchWinner } from '@enums/match-winner.enum';
import { ScoreType } from '@enums/score-type.enum';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { Subject } from 'rxjs';
import { IPlayerNameAndId } from '@entities/request/iPlayerNameAndId';
import { ScoreCalculations } from '@shared/engine/score.calculations';

// Note: do not try to split the MatchWinner logic into a new component.
@Component({
  selector: 'app-match-score',
  templateUrl: './match-score.component.html',
  styleUrls: ['./match-score.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchScoreComponent implements OnInit {
  MatchPlayer = MatchPlayer;
  MatchWinner = MatchWinner;
  ScoreType = ScoreType;

  @Input() mustValidate!: Subject<number>;
  @Input() opponent?: IPlayerNameAndId | null;
  @Input() sets!: Set[];
  @Output() setsChange = new EventEmitter<Set[]>();
  @Output() winnerChange = new EventEmitter<MatchWinner>();
  @Input() winner?: MatchWinner;
  user$ = this.accountService.user$;

  setsCount = 1;
  setIndexCurrent = 0;
  setIndexLast = 0;
  setInputs: (number | null)[][][] = this.initSetInputs();
  setInputsValid: (boolean | null)[] = [null, null, null];
  setInputsValidText: (string | null)[] = [null, null, null];

  private winnerText?: string;
  private notifiedCount = -7;

  constructor(
    private accountService: AccountService,
    private notifier: NotifierWrapService
  ) {}

  ngOnInit(): void {
    this.setInputsFromSet();
  }

  isDisabled(setIndex: number): boolean {
    return (
      setIndex >= this.setsCount || this.isInputValid(setIndex - 1) !== true
    );
  }

  isActive(setIndex: number): boolean {
    return setIndex === this.setIndexCurrent;
  }

  isInputValid(setIndex: number): boolean {
    return this.setInputsValid[setIndex] !== false;
  }

  onFocus(setIndex: number): void {
    if (this.isActive(setIndex) || this.isDisabled(setIndex)) return;
    this.setIndexLast = this.setIndexCurrent;
    this.setIndexCurrent = setIndex;

    this.setSetInputValid(this.setIndexLast);
    this.notifyIfSetIsInvalid(this.setIndexLast);
    this.emitSets();
    this.updateWinner();
  }

  public onSetInput(
    player: MatchPlayer,
    setIndex: number,
    type: ScoreType,
    result: number | null
  ): void {
    this.setSetInput(player, setIndex, type, result);

    if (this.isSetInputValid(setIndex)) {
      this.setInputsValid[setIndex] = true;
      this.updateSet(setIndex);
      this.updateWinner();
    } else {
      this.setInputsValid[setIndex] = false;
    }

    this.emitSets();
    this.notifiedCount = -7;
  }

  public onWinnerInput(winner: MatchWinner): void {
    this.updateWinner(winner);
    this.notifyWinner();
  }

  public getSetInput(
    player: MatchPlayer,
    set: number,
    type: ScoreType
  ): number | null {
    return this.setInputs[set][player - 1][type];
  }

  private setSetInput(
    player: MatchPlayer,
    setIndex: number,
    type: ScoreType,
    result: number | null
  ): void {
    this.setInputs[setIndex][player - 1][type] = result;
    this.manageSetInputs(setIndex);
  }

  private emitSets(): boolean {
    if (this.setInputsValid.includes(false)) {
      this.setsChange.emit([]);
      return false;
    } else {
      this.setInputs.forEach((_, index) => {
        this.updateSet(index);
      });
      this.setsChange.emit(this.sets);
      return true;
    }
  }

  public notifyIfSetIsInvalid(set: number): void {
    if (!this.setInputsValid[set]) {
      const message = this.setInputsValidText[set];
      if (!message) return;
      this.notifier.notify(message, 'red');
    }
  }

  private updateWinner(winner?: MatchWinner): void {
    const calculated = ScoreCalculations.CalculateWinner(this.sets);
    this.winner = winner != null ? winner : calculated;

    this.winnerText = ScoreCalculations.GetWinnerText(
      this.winner,
      calculated,
      this.opponent?.firstName || 'Opponent'
    );

    this.winnerChange.emit(this.winner);
  }

  private updateSet(setIndex: number): void {
    try {
      const set = new Set(
        this.setInputs[setIndex][0][ScoreType.Games],
        this.setInputs[setIndex][1][ScoreType.Games],
        this.setInputs[setIndex][0][ScoreType.Points],
        this.setInputs[setIndex][1][ScoreType.Points]
      );
      if (this.sets[setIndex] !== null || this.sets[setIndex] !== undefined) {
        this.sets[setIndex] = set;
      } else {
        this.sets.push(set);
      }
    } catch (error) {
      return;
    }
  }

  private setInputsFromSet(): void {
    if (
      this.sets === null ||
      this.sets === undefined ||
      this.sets.length === 0
    ) {
      return;
    }
    const sets = this.sets;

    for (let i = 0; i < sets.length; i++) {
      this.onSetInput(MatchPlayer.One, i, ScoreType.Games, sets[i].gamesP1);
      this.onSetInput(MatchPlayer.One, i, ScoreType.Points, sets[i].pointsP1);
      this.onSetInput(MatchPlayer.Two, i, ScoreType.Games, sets[i].gamesP2);
      this.onSetInput(MatchPlayer.Two, i, ScoreType.Points, sets[i].pointsP2);
    }
    this.emitSets();
  }

  private setSetInputValid(set: number): void {
    this.setInputsValid[set] = this.isSetInputValid(set);
  }

  private isSetInputValid(setIndex: number): boolean {
    const isValid = Set.Validate(
      this.setInputs[setIndex][0][ScoreType.Games],
      this.setInputs[setIndex][1][ScoreType.Games],
      this.setInputs[setIndex][0][ScoreType.Points],
      this.setInputs[setIndex][1][ScoreType.Points],
      'You',
      this.opponent?.firstName
    );

    this.setInputsValidText[setIndex] = `Set ${setIndex + 1}: ${
      isValid.reason ||
      "Something isn't right about the set, but I can't figure out what it is."
    }`;

    return isValid.valid;
  }

  private manageSetInputs(setIndex: number): void {
    if (setIndex + 1 === this.setsCount) {
      this.setsCount++;
      if (this.setsCount > this.setInputsValid.length) {
        this.setInputsValid.push(null);
      }

      if (this.setsCount > this.setInputs.length) {
        this.setInputs.push([
          [null, null],
          [null, null],
        ]);
      }
    }
  }

  private initSetInputs(): (number | null)[][][] {
    return [
      [
        [null, null],
        [null, null],
      ],
      [
        [null, null],
        [null, null],
      ],
      [
        [null, null],
        [null, null],
      ],
    ];
  }

  private notifyWinner(): void {
    if (!this.winnerText) return;
    if (this.notifiedCount < 1 || this.notifiedCount > 15) {
      this.notifier.notify(`${this.winnerText}.`);
    } else {
      switch (this.notifiedCount) {
        case 1:
          this.notifier.notify('Trouble deciding?');
          break;
        case 2:
          this.notifier.notify(`${this.winnerText}.`);
          break;
        case 3:
          this.notifier.notify(`${this.winnerText}.`);
          break;
        case 4:
          this.notifier.notify(`${this.winnerText}.`);
          break;
        case 5:
          this.notifier.notify('Make your choice!');
          break;
        case 8:
          this.notifier.notify('Stop it');
          break;
        case 9:
          this.notifier.notify('Stop.');
          break;
        case 10:
          this.notifier.notify('Pls', 'orange');
          break;
        case 13:
          this.notifier.notify("I'm logging your name.");
          break;
        default:
          break;
      }
    }
    this.notifiedCount++;
  }
}
