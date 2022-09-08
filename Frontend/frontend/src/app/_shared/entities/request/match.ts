import { Set } from './set';
import { Duration } from './duration';
import { MatchWinner } from '@enums/match-winner.enum';
import { MatchType } from '@enums/match-type.enum';
import { RequestBase } from './requestBase';
import { IMatchFull } from '@entities/response/iMatchFull';

export class Match extends RequestBase {
  constructor(
    public id: number | undefined,
    public playerOneId: string,
    public playerTwoId: string,
    public sets: Set[],
    public type: MatchType,
    public winner: MatchWinner,
    public ranked: boolean,
    public duration: Duration,
    public surfaceId: number,
    public playerOnePartnerId?: string,
    public playerTwoPartnerId?: string,
    public clubId?: number
  ) {
    super();
  }

  public static CreateMatch(match: IMatchFull): Match {
    return new Match(
      match.id,
      match.playerOneId,
      match.playerTwoId,
      Set.CreateSets(match.sets),
      match.type,
      match.winner,
      match.ranked,
      Duration.Create(match.duration),
      match.surface.id,
      match.playerOnePartnerId,
      match.playerTwoPartnerId,
      match.club?.id
    );
  }
}
