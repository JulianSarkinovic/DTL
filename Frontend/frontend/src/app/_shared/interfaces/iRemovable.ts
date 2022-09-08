import { Status } from '@enums/status.enum';
import { IEntity } from './iEntity';

export interface IRemovable<T> extends IEntity<T> {
  status: Status;
}
