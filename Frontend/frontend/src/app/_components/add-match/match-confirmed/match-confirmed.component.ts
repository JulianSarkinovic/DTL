import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-match-confirmed',
  templateUrl: './match-confirmed.component.html',
  styleUrls: ['./match-confirmed.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchConfirmedComponent implements OnInit {
  title?: string;
  text?: string;

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.setTitle(params.result);
      this.setText(params.result);
    });

    setTimeout(() => {
      void this.router.navigate(['ranking']);
    }, 5000);
  }

  setTitle(result: string): void {
    switch (result) {
      case 'match-not-found' || 'token-not-matched':
        this.title = 'Failed.';
        break;
      case 'already-declined':
        this.title = 'Again?';
        break;
      case 'already-confirmed':
        this.title = 'Again?';
        break;
      case 'failed':
        this.title = 'Failed.';
        break;
      case 'confirmed':
        this.title = 'Nice';
        break;
      case 'declined':
        this.title = 'Ok then';
        break;
      default:
        this.title = 'Oops.';
        break;
    }
  }

  setText(result: string): void {
    switch (result) {
      case 'match-not-found' || 'token-not-matched':
        this.text = 'Something is wrong with the link.';
        break;
      case 'already-declined':
        this.text = 'This match was already declined.';
        break;
      case 'already-confirmed':
        this.text = 'This match was already confirmed.';
        break;
      case 'failed':
        this.text = 'Confirming the match failed. Strange.';
        break;
      case 'confirmed':
        this.text = 'The match has been confirmed!.';
        break;
      case 'declined':
        this.title = 'The match was successfully declined.';
        break;
      default:
        this.text = 'Something went wrong. Was the link changed, possibly?';
        break;
    }
  }
}
