export class PlayerResetPasswordModel {
  constructor(
    public email: string,
    public token: string,
    public newPassword: string,
    public confirmationPassword: string
  ) {}
}
