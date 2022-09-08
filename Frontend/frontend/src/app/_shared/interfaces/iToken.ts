import { Role } from '@enums/role.enum';

export interface IToken {
  exp: number;
  iat: number;
  nbf: number;
  name: string;
  role: Role[];
}
