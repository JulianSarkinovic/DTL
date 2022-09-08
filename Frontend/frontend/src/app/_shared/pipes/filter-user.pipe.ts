import { Pipe, PipeTransform } from '@angular/core';
import { TokenService } from '@services/auth-token.service';
import { IPlayer } from '@entities/response/iPlayer';

@Pipe({
  name: 'filterUser',
})
export class FilterUserPipe implements PipeTransform {
  transform(value?: IPlayer[] | null): IPlayer[] | null {
    if (!value) return null;
    const user = TokenService.GetUser();
    return user ? value.filter((player) => player.id !== user.id) : value;
  }
}
