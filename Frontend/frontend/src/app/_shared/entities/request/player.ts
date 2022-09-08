import { IRequestBase } from './iRequestBase';
import { Gender } from '@enums/gender.enum';
import { Role } from '@enums/role.enum';
import { Status } from '@enums/status.enum';

export class Player implements IRequestBase<string> {
  constructor(
    public firstName: string,
    public lastName: string,
    public email: string,
    public gender?: Gender,
    public status?: Status,
    public dateOfBirth?: Date,
    public roles?: Role[],
    public phoneNumber?: string
  ) {}

  public id?: string;
}
