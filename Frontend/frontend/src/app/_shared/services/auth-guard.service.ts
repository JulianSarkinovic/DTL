import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { Role } from '@enums/role.enum';
import { TokenService } from '@services/auth-token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService implements CanActivate {
  constructor(private router: Router, private tokenService: TokenService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const token = this.tokenService.getToken();
    // Info: The route requires authorization, check token.
    if (!token) {
      void this.router.navigate(['login'], {
        queryParams: { returnUrl: state.url },
      });
      return false;
    }
    // Info: The route requires a role, check user's roles.
    if (route.data.roles && !this.UserHasRole(route.data.roles, token.role)) {
      void this.router.navigate(['']);
      return false;
    }
    return true;
  }

  private UserHasRole = (requiredRoles: Role[], usersRoles: Role[]): boolean =>
    requiredRoles.filter((role) => usersRoles.includes(role)).length !== 0;
}
