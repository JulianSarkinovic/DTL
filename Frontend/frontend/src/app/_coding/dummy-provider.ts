import { IPlayer } from '../_shared/entities/response/iPlayer';
import { Gender } from '../_shared/enums/gender.enum';
import { Role } from '../_shared/enums/role.enum';
import { Status } from '../_shared/enums/status.enum';

export class DummyProvider {
  static GetIPlayer(): IPlayer {
    return {
      id: '21ednjf',
      dateOfBirth: new Date(),
      firstName: 'John',
      lastName: 'Doe',
      roles: [Role.Developer],
      gender: Gender.Male,
      status: Status.Enabled,
    };
  }
}
