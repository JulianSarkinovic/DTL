export class IsValid {
  private constructor(valid: boolean, reason?: string) {
    this.valid = valid;
    this.reason = reason;
  }

  valid: boolean;
  reason?: string;

  static Yes(): IsValid {
    return new IsValid(true);
  }

  static No(reason: string): IsValid {
    return new IsValid(false, reason);
  }
}
