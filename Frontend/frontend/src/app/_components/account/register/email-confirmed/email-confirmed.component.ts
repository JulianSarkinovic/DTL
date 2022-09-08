import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-email-confirmed',
  templateUrl: './email-confirmed.component.html',
  styleUrls: ['./email-confirmed.component.scss'],
})
export class EmailConfirmedComponent implements OnInit {
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
    }, 4000);
  }

  setTitle(result: string): void {
    switch (result) {
      case 'declined':
        this.title = 'Allright';
        break;
      case 'user-not-found':
        this.title = 'Failed.';
        break;
      case 'already-confirmed':
        this.title = 'Again?';
        break;
      case 'failed':
        this.title = 'Failed.';
        break;
      case 'success':
        this.title = 'Success';
        break;
      default:
        this.title = 'Oops.';
        break;
    }
  }

  setText(result: string): void {
    switch (result) {
      case 'declined':
        this.text =
          'We have declined confirmation of your email and removed the associated account.';
        break;
      case 'user-not-found':
        this.text = 'Something is wrong with the link.';
        break;
      case 'already-confirmed':
        this.text = 'This email was already confirmed.';
        break;
      case 'failed':
        this.text = 'Confirming your email failed. Strange..';
        break;
      case 'success':
        this.text = 'Your email has been confirmed!';
        break;
      default:
        this.text = 'Something went wrong, was the link changed somehow?';
        break;
    }
  }
}
