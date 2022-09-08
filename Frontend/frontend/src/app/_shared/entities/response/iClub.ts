import { IResponseBase } from './iResponseBase';

export interface IClub extends IResponseBase<number> {
  name: string;
  registrationNumber?: number;
  surfacesIds: number[];
}
