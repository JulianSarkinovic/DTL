import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPlayerForProfile } from '@entities/response/iPlayerForProfile';
import { PlayerService } from '@services/player.service';
import { Observable } from 'rxjs';

@Component({
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent {
  player$?: Observable<IPlayerForProfile | undefined>;

  constructor(
    private route: ActivatedRoute,
    private playerService: PlayerService
  ) {
    this.route.queryParams.subscribe((params) => {
      this.player$ = this.playerService.getForProfile(params.id as string);
    });
  }
}
