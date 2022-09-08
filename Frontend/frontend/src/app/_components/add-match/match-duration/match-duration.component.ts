import { Options } from '@angular-slider/ngx-slider';
import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { Duration } from 'src/app/_shared/entities/request/duration';

@Component({
  selector: 'app-match-duration',
  templateUrl: './match-duration.component.html',
  styleUrls: ['./match-duration.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchDurationComponent implements OnChanges, OnInit {
  @Output() durationChange = new EventEmitter<Duration>();
  @Input() duration?: Duration;
  @Input() disabled = false;

  dateForm = new FormControl(new Date().toISOString());
  floor = 300;
  start = 720;
  end = 840;

  options: Options = {
    translate: (value: number) => this.convertMinutesToTimeString(value),
    floor: this.floor,
    step: 15,
    ceil: 1440,
  };

  ngOnInit(): void {
    this.onDurationEmit();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.durationFromParent && changes.durationFromParent.currentValue) {
      const duration = changes.durationFromParent.currentValue as Duration;
      const year = duration.start.getFullYear();
      const month = duration.start.getMonth();
      const day = duration.start.getDate();
      const date = new Date(year, month, day);
      this.dateForm.setValue(date.toISOString());
      this.start = this.getDifferenceInMinutes(date, duration.start);
      this.end = this.getDifferenceInMinutes(date, duration.end);
    }
  }

  public onDurationEmit(): void {
    const durationForEmission = this.getDuration(this.dateForm.value);
    this.durationChange.emit(durationForEmission);
  }

  private getDuration(dateISO?: string): Duration {
    const date = dateISO ? new Date(dateISO) : new Date();
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);

    const startDate = new Date(date.getTime() + this.start * 60000);
    const endDate = new Date(date.getTime() + this.end * 60000);
    return new Duration(startDate, endDate);
  }

  private convertMinutesToTimeString = (mins: number): string => {
    const hours = Math.floor(mins / 60);
    const minutes = mins % 60;
    const hoursString = hours < 10 ? `0${hours}` : hours.toString();
    const minutesString = minutes < 10 ? `0${minutes}` : minutes.toString();
    return `${hoursString}:${minutesString}`;
  };

  private getDifferenceInMinutes = (base: Date, compare: Date) => {
    const diff = compare.getTime() - base.getTime();
    return Math.floor(diff / 60000);
  };
}
