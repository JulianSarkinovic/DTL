import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  ngOnInit(): void {
    this.onResizeHeight(window.innerHeight);
    this.onResizeWidth(window.innerWidth);
  }

  @HostListener('window:resize', ['$event.target.innerHeight'])
  onResizeHeight(height: number): void {
    const vh = height * 0.01;
    document.documentElement.style.setProperty('--vh', `${vh}px`);
  }

  @HostListener('window:resize', ['$event.target.innerWidth'])
  onResizeWidth(width: number): void {
    const vw = width * 0.01;
    document.documentElement.style.setProperty('--vw', `${vw}px`);
  }
}
