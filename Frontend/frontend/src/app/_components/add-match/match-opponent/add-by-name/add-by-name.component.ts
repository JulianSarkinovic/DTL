import {
  ChangeDetectionStrategy,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Component } from '@angular/core';
import { PlayerService } from '@services/player.service';
import { Step } from 'src/app/_shared/enums/step.enum';
import { IPlayerNameAndId } from '@entities/request/iPlayerNameAndId';

@Component({
  selector: 'app-match-opponent-add-by-name',
  templateUrl: './add-by-name.component.html',
  styleUrls: ['./add-by-name.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddByNameComponent {
  @Output() opponentChange = new EventEmitter<IPlayerNameAndId>();
  @Input() opponent?: IPlayerNameAndId;
  @Output() stepEvent = new EventEmitter<Step>();

  players$ = this.playerService.data$;
  isSearching$ = this.playerService.isSearching$;
  isLoading$ = this.playerService.isLoading$;

  constructor(private playerService: PlayerService) {}

  onSearch(value: string): void {
    this.playerService.search(value);
  }

  onOpponentSelect(opponent: IPlayerNameAndId): void {
    opponent
      ? this.opponentChange.emit(opponent)
      : this.stepEvent.emit(Step.Stay);
  }

  onOpponentDisplay(opponent?: IPlayerNameAndId): string {
    return opponent ? `${opponent.firstName} ${opponent.lastName}` : '';
  }
}
