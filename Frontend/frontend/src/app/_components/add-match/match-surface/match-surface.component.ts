import {
  ChangeDetectionStrategy,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { Component } from '@angular/core';
import { SurfaceService } from '@services/surface.service';
import { ISurface } from 'src/app/_shared/entities/response/iSurface';

@Component({
  selector: 'app-match-surface',
  templateUrl: './match-surface.component.html',
  styleUrls: ['./match-surface.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MatchSurfaceComponent {
  @Output() surfaceChange = new EventEmitter<ISurface>();
  @Input() surface?: ISurface;
  @Input() disabled = false;

  surfaces$ = this.surfaceService.surfaces$;

  constructor(private surfaceService: SurfaceService) {}

  onSurfaceSearch(value: string): void {
    this.surfaceService.filter(value);
  }

  onSurfaceDisplay = (surface?: ISurface): string =>
    surface ? `${surface.name}` : '';

  onSurfaceSelect(surface: ISurface): void {
    this.surfaceChange.emit(surface);
  }
}
