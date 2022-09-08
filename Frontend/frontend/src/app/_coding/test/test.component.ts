import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NotifierWrapService } from '@services/notifier-wrap.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TestComponent {
  constructor(private notifier: NotifierWrapService) {
    this.notify();
  }

  notify(): void {
    this.notifier.notify(
      'Here is a note with a decent amount of text in the notification.',
      'orange'
    );
  }
}
