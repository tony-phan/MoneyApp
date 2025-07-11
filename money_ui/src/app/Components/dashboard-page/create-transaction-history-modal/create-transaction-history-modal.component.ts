import { Component } from '@angular/core';
import { MaterialModule } from '../../../material.module';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSelect } from "@angular/material/select";
import { MONTHS } from '../../../constants';

@Component({
  selector: 'app-create-transaction-history-modal',
  standalone: true,
  imports: [MaterialModule, CommonModule, ReactiveFormsModule, MatSelect],
  templateUrl: './create-transaction-history-modal.component.html',
  styleUrl: './create-transaction-history-modal.component.css'
})
export class CreateTransactionHistoryModalComponent {
  monthOptions = MONTHS;
  form: FormGroup;
  
  constructor(
    private fb: FormBuilder, 
    private dialogRef: MatDialogRef<CreateTransactionHistoryModalComponent>
  ) {
    this.form = this.fb.group({
      month: ['', [Validators.required]],
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
