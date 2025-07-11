import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { EXPENSE_CATEGORIES, INCOME_CATEGORIES, TRANSACTION_TYPES } from '../../constants';
import { MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-create-transaction-modal',
  standalone: true,
  imports: [ReactiveFormsModule, MaterialModule],
  templateUrl: './create-transaction-modal.component.html',
  styleUrl: './create-transaction-modal.component.css'
})
export class CreateTransactionModalComponent {
  form: FormGroup;

  transactionTypes = TRANSACTION_TYPES;
  incomeCategories = INCOME_CATEGORIES;
  expenseCategories = EXPENSE_CATEGORIES;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateTransactionModalComponent>
  ) {
    this.form = this.fb.group({
      amount: [''],
      transactionType: [''],
      incomeCategory: [''],
      expenseCategory: [''],
      description: [''],
      date: [''],
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