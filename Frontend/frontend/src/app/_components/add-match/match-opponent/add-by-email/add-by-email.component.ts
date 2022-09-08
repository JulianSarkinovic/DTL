import {
  ChangeDetectionStrategy,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { Component } from '@angular/core';
import { IPlayerCreationModel } from '@entities/request/iPlayerCreationModel';
import { PlayerService } from '@services/player.service';
import { NgForm } from '@angular/forms';
import { IPlayerNameAndId } from '@entities/request/iPlayerNameAndId';

@Component({
  selector: 'app-match-opponent-add-by-mail',
  templateUrl: './add-by-email.component.html',
  styleUrls: ['./add-by-email.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddByEmailComponent {
  @ViewChild('addByEmailForm') form!: NgForm;
  @Output() opponentChange = new EventEmitter<IPlayerNameAndId>();
  @Input() opponent?: IPlayerNameAndId;

  isSubmitting$ = this.playerService.isSubmitting$;

  constructor(private playerService: PlayerService) {}

  // Todo: email should not be players own email.
  //   Notifier: it looks like you are trying to do something that isn't possible.
  //     If you think that it could and should be possible, let us know info@dtl.com
  findOrCreate(opponentModel: IPlayerCreationModel): void {
    this.playerService
      .findOrCreate(opponentModel)
      .subscribe((player) => this.opponentChange.emit(player));
  }
}
