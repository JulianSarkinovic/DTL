import {
  ChangeDetectionStrategy,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ClubService } from '@services/club.service';
import { NotifierWrapService } from '@services/notifier-wrap.service';
import { combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { Club } from 'src/app/_shared/entities/request/club';
import { IClub } from 'src/app/_shared/entities/response/iClub';

@Component({
  selector: 'app-match-club',
  templateUrl: './match-club.component.html',
  styleUrls: ['./match-club.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchClubComponent {
  @Output() clubChange = new EventEmitter<IClub>();
  @Input() club?: IClub;
  @Input() disabled = false;

  clubNameInput?: string;
  clubForm = new FormControl();

  vm$ = combineLatest([
    this.clubService.data$,
    this.clubService.isSearching$,
  ]).pipe(
    map(([clubs, isSearchingClubs]) => ({
      clubs,
      isSearchingClubs,
    }))
  );

  constructor(
    private clubService: ClubService,
    private notifier: NotifierWrapService
  ) {}

  // Todo: using the disabled attribute on an reactive forms control is not a great idea apparently.
  // Use the component and then check the console for more information.
  getClubNameInput = (): string => this.clubNameInput || '';

  onClubSearch(value: string): void {
    this.clubNameInput = value;
    this.clubService.search(value);
  }

  onClubDisplay = (club?: IClub): string => (club ? club.name : '');

  onClubSelect(club: IClub | null): void {
    if (club) {
      this.clubChange.emit(club);
    } else {
      if (!this.clubNameInput || this.clubNameInput.length < 3) {
        this.notifier.notify(
          "I can't add that club. Please enter the actual name of the club you've played the match at.",
          'red'
        );
        return;
      }
      this.clubService
        .add(new Club(this.clubNameInput))
        .subscribe((c: IClub) => this.clubChange.emit(c));
    }
  }
}
