import { IEloHistory } from './iEloHistory';
import { IResponseBase } from './iResponseBase';

export interface IElo extends IResponseBase<number> {
  rating: number;
  isProvisional: boolean;
  history?: IEloHistory[];
}
