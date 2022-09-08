import { Injectable } from '@angular/core';
import { IEnvironment } from 'src/environments/ienvironment';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class EnvironmentService implements IEnvironment {
  get production(): boolean {
    return environment.production;
  }

  get urlAPI(): string {
    return environment.urlAPI;
  }

  get urlAPIBase(): string {
    return environment.urlAPIBase;
  }
}
