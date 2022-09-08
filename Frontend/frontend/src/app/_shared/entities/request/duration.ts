import { IDuration } from '@entities/response/iDuration';

export class Duration {
  constructor(start: Date, end: Date) {
    if (start > end) {
      throw new RangeError('Start date is greater than end date.');
    }
    this.start = start;
    this.end = end;

    this.durationInMinutes =
      this.end.getMinutes() +
      this.end.getHours() * 60 -
      this.start.getMinutes() -
      this.start.getHours() * 60;
  }

  public start: Date;
  public end: Date;
  public durationInMinutes: number;

  public static Create(duration: IDuration): Duration {
    return new Duration(new Date(duration.start), new Date(duration.end));
  }
}
