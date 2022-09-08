import { Gender } from '@enums/gender.enum';
import { Role } from '@enums/role.enum';
import { Status } from '@enums/status.enum';

export class playerCreationModel {
  constructor(
    public firstName: string,
    public lastName: string,
    public email: string,
    public password: string,
    public confirmationPassword: string,
    public roles?: Role[],
    public gender?: Gender,
    public status?: Status,
    public dateOfBirth?: Date,
    public phoneNumber?: string
  ) {}
}
