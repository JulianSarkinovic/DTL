import { ISet } from './iSet';
import { MatchWinner } from '@enums/match-winner.enum';
import { MatchFormat } from '@enums/match-format.enum';
import { MatchType } from '@enums/match-type.enum';
import { IResponseBase } from './iResponseBase';
import { ISurface } from './iSurface';
import { IClub } from './iClub';
import { IDuration } from './iDuration';
import { ConfirmationStatus } from '@enums/confirmation-status.enum';

export interface IMatchFull extends IResponseBase<number> {
  playerOneId: string;
  playerOneFirstName: string;
  playerOneLastName: string;

  playerTwoId: string;
  playerTwoFirstName: string;
  playerTwoLastName: string;

  playerOnePartnerId?: string;
  playerOnePartnerFirstName?: string;
  playerOnePartnerLastName?: string;

  playerTwoPartnerId?: string;
  playerTwoPartnerFirstName?: string;
  playerTwoPartnerLastName?: string;

  surface: ISurface;
  club?: IClub;

  sets: ISet[];
  duration: IDuration;
  winner: MatchWinner;
  format: MatchFormat;
  type: MatchType;
  ranked: boolean;
  confirmationStatus: ConfirmationStatus;
}
