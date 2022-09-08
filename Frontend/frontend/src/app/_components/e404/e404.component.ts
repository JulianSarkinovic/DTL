import { Component } from '@angular/core';

@Component({
  templateUrl: './e404.component.html',
  styleUrls: ['./e404.component.scss'],
})
export class E404Component {
  errorMessage = `We can't find the page you're looking for.`;
}
