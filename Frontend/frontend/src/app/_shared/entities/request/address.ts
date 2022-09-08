import { RequestBase } from './requestBase';

export class Address extends RequestBase {
  constructor(
    public country: string,
    public city: string,
    public streetAddress: string,
    public postalCode: string,
    public region?: string,
    public firstName?: string,
    public lastName?: string,
    public companyName?: string,
    public playerId?: string,
    public clubId?: number
  ) {
    super();
  }
}
