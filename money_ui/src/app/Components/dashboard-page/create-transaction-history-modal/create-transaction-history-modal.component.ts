import { Component } from '@angular/core';
import { MaterialModule } from '../../../material.module';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-transaction-history-modal',
  standalone: true,
  imports: [MaterialModule, CommonModule, ReactiveFormsModule],
  templateUrl: './create-transaction-history-modal.component.html',
  styleUrl: './create-transaction-history-modal.component.css'
})
export class CreateTransactionHistoryModalComponent {
  form: FormGroup;

  constructor(
    private fb: FormBuilder, 
    private dialogRef: MatDialogRef<CreateTransactionHistoryModalComponent>
  ) {
    this.form = this.fb.group({
      month: ['', [Validators.required, Validators.min(1), Validators.max(12)]],
      year: ['', [Validators.required, Validators.min(1900), Validators.max(2100)]],
    });
  }

  submit(): void {
    if(this.form.valid) 
      this.dialogRef.close(this.form.value);
  }

  cancel(): void {
    this.dialogRef.close(null);
  }
}
