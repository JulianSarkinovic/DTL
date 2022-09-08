export class PlayerChangePasswordModel {
  constructor(
    public email: string,
    public currentPassword: string,
    public newPassword: string,
    public confirmationPassword: string
  ) {}
}
