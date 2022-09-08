import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-match-confirmation-dialog',
  templateUrl: './match-confirmation-dialog.component.html',
  styleUrls: ['./match-confirmation-dialog.component.scss'],
})
export class MatchConfirmationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<MatchConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public opponentName?: string
  ) {}

  onEdit(): void {
    this.dialogRef.close(true);
  }

  onDecline(): void {
    this.dialogRef.close(false);
  }
}
