import { RequestBase } from './requestBase';

export class Friendship extends RequestBase {
  constructor(public friender: string, public friended: string) {
    super();
  }
}
