import { Address } from './address';
import { RequestBase } from './requestBase';

export class Club extends RequestBase {
  constructor(
    name: string,
    registrationNumber?: number,
    surfaces?: number[],
    addresses?: Address[]
  ) {
    super();
    this.name = name;
    this.registrationNumber = registrationNumber;
    this.surfaces = surfaces;
    this.addresses = addresses;
  }

  name: string;
  registrationNumber?: number;
  surfaces?: number[];
  addresses?: Address[];
}
