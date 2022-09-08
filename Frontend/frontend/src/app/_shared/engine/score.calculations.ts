import { BonusConstants as Bonus } from '../constants/bonus.constants';
import { Set } from '@entities/request/set';
import { MatchPlayer } from '@enums/match-player.enum';
import { MatchWinner } from '@enums/match-winner.enum';

export class ScoreCalculations {
  public static CalculateWinner(sets: Set[]): MatchWinner {
    let setsPlayerOne = 0;
    let setsPlayerTwo = 0;

    for (const set of sets) {
      if (!set) continue;
      if (set.gamesP1 && set.gamesP2) {
        setsPlayerOne += set.gamesP1 > set.gamesP2 ? 1 : 0;
        setsPlayerTwo += set.gamesP2 > set.gamesP1 ? 1 : 0;
      }
      if (set.pointsP1 && set.pointsP2) {
        setsPlayerOne +=
          set.isSuperTieBreaker() && set.pointsP1 > set.pointsP2 ? 1 : 0;
        setsPlayerTwo +=
          set.isSuperTieBreaker() && set.pointsP2 > set.pointsP2 ? 1 : 0;
      }
    }

    if (setsPlayerOne === setsPlayerTwo) return MatchWinner.Tie;
    return setsPlayerOne > setsPlayerTwo
      ? MatchWinner.PlayerOne
      : MatchWinner.PlayerTwo;
  }

  public static CalculateScoreTotals(
    sets: Set[],
    winner?: MatchPlayer
  ): [number, number] {
    let scorePlayerOne = 0;
    let scorePlayerTwo = 0;

    for (const set of sets) {
      if (set.gamesP1 && set.gamesP2) {
        scorePlayerOne += set.gamesP1 * Bonus.Game;
        scorePlayerTwo += set.gamesP2 * Bonus.Game;
        scorePlayerOne += set.gamesP1 > set.gamesP2 ? Bonus.Set : 0;
        scorePlayerTwo += set.gamesP2 > set.gamesP1 ? Bonus.Set : 0;
      }

      if (set.pointsP1 && set.pointsP2) {
        scorePlayerOne +=
          set.pointsP1 > set.pointsP2 && set.isTieBreaker()
            ? Bonus.TieBreaker
            : 0;
        scorePlayerTwo +=
          set.pointsP2 > set.pointsP2 && set.isTieBreaker()
            ? Bonus.TieBreaker
            : 0;
        scorePlayerOne +=
          set.pointsP1 > set.pointsP2 && set.isSuperTieBreaker()
            ? Bonus.SuperTieBreaker
            : 0;
        scorePlayerTwo +=
          set.pointsP2 > set.pointsP2 && set.isSuperTieBreaker()
            ? Bonus.SuperTieBreaker
            : 0;
      }
    }

    if (winner) {
      scorePlayerOne += winner === MatchPlayer.One ? Bonus.Match : 0;
      scorePlayerTwo += winner === MatchPlayer.Two ? Bonus.Match : 0;
    }

    return [scorePlayerOne, scorePlayerTwo];
  }

  public static GetWinnerText(
    providedWinner: MatchWinner | null | undefined,
    calcWinner: MatchWinner,
    opponentName: string
  ): string {
    const provided = providedWinner !== null;
    const agreement = providedWinner === calcWinner;

    const decap = (s: string) => s.charAt(0).toLowerCase() + s.slice(1);
    const tie = 'It was a tie';
    const him = () => `The winner is ${opponentName}`;
    const you = () => `You've won`;
    const looks = (result: string) => `It looks like ${decap(result)}`;
    const sure = (result: string, looksLike: string) =>
      `Are you sure ${decap(result)}? ${looksLike}`;

    if (!provided && calcWinner == null) {
      return 'There seems to be no winner';
    } else if (!provided && calcWinner === MatchWinner.Tie) {
      return looks(tie);
    } else if (!provided && calcWinner === MatchWinner.PlayerOne) {
      return looks(you());
    } else if (!provided) {
      return looks(him());
    } else if (agreement && providedWinner === MatchWinner.Tie) {
      return tie;
    } else if (agreement && providedWinner === MatchWinner.PlayerOne) {
      return `${you()} :)`;
    } else if (agreement) {
      return him();
    } else if (
      !agreement &&
      providedWinner === MatchWinner.Tie &&
      calcWinner === MatchWinner.PlayerOne
    ) {
      return sure(tie, looks(you()));
    } else if (
      !agreement &&
      providedWinner === MatchWinner.Tie &&
      calcWinner === MatchWinner.PlayerTwo
    ) {
      return sure(tie, looks(him()));
    } else if (
      !agreement &&
      providedWinner === MatchWinner.PlayerOne &&
      calcWinner === MatchWinner.PlayerTwo
    ) {
      return sure(you(), looks(him()));
    } else if (
      !agreement &&
      providedWinner === MatchWinner.PlayerOne &&
      calcWinner === MatchWinner.Tie
    ) {
      return sure(you(), looks(tie));
    } else if (
      !agreement &&
      providedWinner === MatchWinner.PlayerTwo &&
      calcWinner === MatchWinner.PlayerOne
    ) {
      return sure(him(), looks(you()));
    } else if (
      !agreement &&
      providedWinner === MatchWinner.PlayerTwo &&
      calcWinner === MatchWinner.Tie
    ) {
      return sure(him(), looks(tie));
    }

    return '';
  }
}
