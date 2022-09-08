import { ISet } from './iSet';
import { MatchWinner } from '@enums/match-winner.enum';
import { MatchFormat } from '@enums/match-format.enum';
import { MatchType } from '@enums/match-type.enum';
import { IResponseBase } from './iResponseBase';
import { IPlayer } from './iPlayer';
import { IDuration } from './iDuration';

export interface IMatch extends IResponseBase<number> {
  playerOneId: string;
  playerOne?: IPlayer;

  playerTwoId: string;
  playerTwo?: IPlayer;

  playerOnePartnerId?: string;
  playerOnePartner?: IPlayer;

  playerTwoPartnerId?: string;
  playerTwoPartner?: IPlayer;

  surfaceId: number;
  clubId?: number;

  sets: ISet[];
  duration: IDuration;
  winner: MatchWinner;
  format: MatchFormat;
  type: MatchType;
  ranked: boolean;
}
