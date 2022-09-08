import { IResponseBase } from './iResponseBase';

export interface ISet extends IResponseBase<number> {
  gamesP1?: number;
  gamesP2?: number;
  pointsP1?: number;
  pointsP2?: number;
  matchId?: number;
  isTieBreaker: boolean;
  isSuperTieBreaker: boolean;
}
