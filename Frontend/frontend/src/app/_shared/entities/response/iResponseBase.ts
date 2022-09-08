import { IEntity } from '@interfaces/iEntity';

export interface IResponseBase<T> extends IEntity<T> {
  createdAt?: Date;
  updatedAt?: Date;
}
