import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { IPlayer } from '@entities/response/iPlayer';
import { IToken } from '@interfaces/iToken';

const STORAGE_KEY_TOKEN = 'token';
const STORAGE_KEY_USER = 'user';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor(private jwtHelper: JwtHelperService) {}

  public static RemoveUserAndToken(): void {
    localStorage.removeItem(STORAGE_KEY_TOKEN);
    localStorage.removeItem(STORAGE_KEY_USER);
  }

  public static GetTokenString(): string | null {
    return localStorage.getItem(STORAGE_KEY_TOKEN);
  }

  public static SetTokenString(token: string): void {
    localStorage.removeItem(STORAGE_KEY_TOKEN);
    localStorage.setItem(STORAGE_KEY_TOKEN, token);
  }

  public static GetUser(): IPlayer | null {
    const userString = localStorage.getItem(STORAGE_KEY_USER);
    if (!userString) return null;
    return JSON.parse(userString) as IPlayer;
  }

  public static SetUser(user: IPlayer): void {
    localStorage.removeItem(STORAGE_KEY_USER);
    localStorage.setItem(STORAGE_KEY_USER, JSON.stringify(user));
  }

  public isAuthenticated(): boolean {
    const jwt = TokenService.GetTokenString();
    if (!jwt) {
      TokenService.RemoveUserAndToken();
      return false;
    }
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    const token = this.jwtHelper.decodeToken(jwt);
    const expired = this.jwtHelper.isTokenExpired(jwt);

    if (!token || expired) {
      TokenService.RemoveUserAndToken();
      return false;
    }
    return true;
  }

  public getToken(): IToken | null {
    const jwt = localStorage.getItem(STORAGE_KEY_TOKEN);
    if (!jwt) return null;
    return this.jwtHelper.decodeToken(jwt) as IToken;
  }
}
