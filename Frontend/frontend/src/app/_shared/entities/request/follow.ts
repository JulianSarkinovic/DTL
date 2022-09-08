import { RequestBase } from './requestBase';
export class Follow extends RequestBase {
  constructor(public followerId: string, public followedId: string) {
    super();
  }
}
