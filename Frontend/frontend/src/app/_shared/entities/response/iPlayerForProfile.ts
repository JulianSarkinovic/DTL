import { Gender } from '@enums/gender.enum';
import { IElo } from './iElo';
import { IMatchFull } from './iMatchFull';
import { IResponseBase } from './iResponseBase';

export interface IPlayerForProfile extends IResponseBase<string> {
  firstName: string;
  lastName: string;
  dateOfBirth?: Date; // Todo: Check: This might actually come back as a string not the Date object.
  gender: Gender;
  elo: IElo;
  matches: IMatchFull[];
}
