import { Gender } from '@enums/gender.enum';
import { Role } from '@enums/role.enum';
import { Status } from '@enums/status.enum';

export interface IPlayerRegister {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmationPassword: string;
  roles?: Role[];
  gender?: Gender;
  status?: Status;
  dateOfBirth?: Date;
  phoneNumber?: string;
}
