import { IResponseBase } from './iResponseBase';

export interface IEloHistory extends IResponseBase<number> {
  elo: number;
  matchId: number;
}
