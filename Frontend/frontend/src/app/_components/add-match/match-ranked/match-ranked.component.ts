import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';

@Component({
  selector: 'app-match-ranked',
  templateUrl: './match-ranked.component.html',
  styleUrls: ['./match-ranked.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchRankedComponent implements OnInit {
  @Output() rankedChange = new EventEmitter<boolean>();
  @Input() ranked = true;
  @Input() disabled = false;

  ngOnInit(): void {
    this.rankedChange.emit(this.ranked);
  }

  onIsRanked(event: MatSlideToggleChange): void {
    this.rankedChange.emit(event.checked);
  }
}
