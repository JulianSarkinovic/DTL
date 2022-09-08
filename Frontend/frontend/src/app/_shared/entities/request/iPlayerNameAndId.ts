import { IEntity } from '@interfaces/iEntity';

export interface IPlayerNameAndId extends IEntity<string> {
  firstName: string;
  lastName: string;
}
