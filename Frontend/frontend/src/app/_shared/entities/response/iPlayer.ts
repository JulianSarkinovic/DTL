import { Gender } from '@enums/gender.enum';
import { Role } from '@enums/role.enum';
import { IElo } from './iElo';
import { IResponseBase } from './iResponseBase';
import { IRemovable } from '@interfaces/iRemovable';

export interface IPlayer extends IResponseBase<string>, IRemovable<string> {
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth?: Date;
  roles: Role[];
  gender: Gender;
  token?: string;
  elo?: IElo;
}
