export interface IPlayerChangePasswordRequest {
  email: string;
  currentPassword: string;
  newPassword: string;
  confirmationPassword: string;
}
