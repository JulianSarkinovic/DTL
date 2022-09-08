import { Injectable } from '@angular/core';
import { NotifierService } from 'angular-notifier';

@Injectable({
  providedIn: 'root',
})
export class NotifierWrapService {
  constructor(private notifier: NotifierService) {}

  static GetNotifierPosition(): number {
    let margin = 0;
    if (window.outerWidth > window.innerWidth) margin += 20;
    if (window.innerWidth > 400) margin += 5;
    if (window.innerWidth > 800) margin += 10;
    if (window.innerWidth > 1200) margin += 5;
    if (window.innerWidth > 1600) margin += 5;
    return margin;
  }

  notify(
    message: string,
    color: 'white' | 'blue' | 'green' | 'orange' | 'red' | 'black' = 'white'
  ): void {
    let type = 'none';
    switch (color) {
      case 'white':
        type = 'blanco';
        break;
      case 'blue':
        type = 'info';
        break;
      case 'green':
        type = 'success';
        break;
      case 'orange':
        type = 'warning';
        break;
      case 'red':
        type = 'error';
        break;
      case 'black':
        type = 'default';
        break;
    }
    this.notifier.notify(type, message);
  }
}
