import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { IMatchFull } from '@entities/response/iMatchFull';
import { MatchWinner } from '@enums/match-winner.enum';

@Component({
  selector: 'app-match-score-view',
  templateUrl: './match-score-view.component.html',
  styleUrls: ['./match-score-view.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchScoreViewComponent {
  MatchWinner = MatchWinner;
  @Input() match: IMatchFull | undefined;
  @Input() playerOneName: string | undefined;
  @Input() playerTwoName: string | undefined;
}
