/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { RequestBase } from './requestBase';
import { IsValid } from '../isValid';
import { ISet } from '@entities/response/iSet';

export class Set extends RequestBase {
  matchId?: number;

  constructor(
    public gamesP1: number | null,
    public gamesP2: number | null,
    public pointsP1: number | null,
    public pointsP2: number | null
  ) {
    super();
    const isValid = Set.Validate(gamesP1, gamesP2, pointsP1, pointsP2);
    if (!isValid.valid) throw Error(isValid.reason);
  }

  public static Validate(
    gamesP1: number | null | undefined,
    gamesP2: number | null | undefined,
    pointsP1: number | null | undefined,
    pointsP2: number | null | undefined,
    nameP1: string = 'You',
    nameP2: string = 'Your opponent'
  ): IsValid {
    if (this.hasValue(gamesP1) && this.hasNoValue(gamesP2)) {
      return IsValid.No(`${nameP1} can't play games by yourself.`);
    } else if (this.hasValue(gamesP2) && this.hasNoValue(gamesP1)) {
      return IsValid.No(`${nameP2} can't have played games all alone.`);
    } else if (this.hasValue(pointsP1) && this.hasNoValue(pointsP2)) {
      return IsValid.No(`${nameP1} can't play points by yourself.`);
    } else if (this.hasValue(pointsP2) && this.hasNoValue(pointsP1)) {
      return IsValid.No(`${nameP2} can't have played points all alone.`);
    } else if (this.hasNoValue(pointsP1) && this.hasNoValue(gamesP1)) {
      return IsValid.No('A set must have games, points, or both.');
    } else if (this.hasValue(gamesP1) && this.hasValue(gamesP2)) {
      if (
        (+gamesP1! > +gamesP2! + 1 && this.hasValue(pointsP1)) ||
        (+gamesP2! > +gamesP1! + 1 && this.hasValue(pointsP1))
      ) {
        return IsValid.No(
          "There can't be a tie-breaker if there was no tie to break."
        );
      } else if (this.hasValue(pointsP1) && this.hasValue(pointsP2)) {
        if (
          (+gamesP1! > +gamesP2! && +pointsP1! < +pointsP2!) ||
          (+gamesP2! > +gamesP1! && +pointsP2! < +pointsP1!)
        ) {
          return IsValid.No(
            'The winner of the tie-breaker must have also won the set.'
          );
        } else if (
          (+gamesP1! > +gamesP2! && +pointsP1! === +pointsP2!) ||
          (+gamesP2! > +gamesP1! && +pointsP2! === +pointsP1!)
        ) {
          return IsValid.No(
            'The games show that the tie-breaker has a winner, but the tie-breaker shows a tie.'
          );
        } else if (
          (+gamesP1! === +gamesP2! && +pointsP1! + 1 < +pointsP2!) ||
          (+gamesP1! === +gamesP2! && +pointsP2! + 1 < +pointsP1!)
        ) {
          return IsValid.No(
            'The points show that the tie-breaker has a winner, but the games show a tie.'
          );
        }
      }
    }

    return IsValid.Yes();
  }

  public static CreateSet(set: ISet): Set {
    return new Set(
      set.gamesP1 ?? null,
      set.gamesP2 ?? null,
      set.pointsP1 ?? null,
      set.pointsP2 ?? null
    );
  }

  public static CreateSets(sets: ISet[]): Set[] {
    const mySets: Set[] = [];
    sets.forEach((set) => {
      mySets.push(Set.CreateSet(set));
    });

    return mySets;
  }

  private static hasValue(number: number | null | undefined): boolean {
    if (number) {
      return true;
    }
    if (number === 0) {
      return true;
    }

    return false;
  }

  private static hasNoValue(number: number | null | undefined): boolean {
    return !this.hasValue(number);
  }

  public isTieBreaker = (): boolean =>
    Set.hasValue(this.gamesP1) &&
    Set.hasValue(this.gamesP2) &&
    Set.hasValue(this.pointsP1) &&
    Set.hasValue(this.pointsP2);

  public isSuperTieBreaker = (): boolean =>
    Set.hasNoValue(this.gamesP1) &&
    Set.hasNoValue(this.gamesP2) &&
    Set.hasValue(this.pointsP1) &&
    Set.hasValue(this.pointsP2);
}
