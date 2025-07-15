import { Component, computed, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
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
  selectedTransactionType = signal<string | null>(null);
  isIncome = computed(() => this.selectedTransactionType() === 'income');
  isExpense = computed(() => this.selectedTransactionType() === 'expense');

  readonly transactionTypes = TRANSACTION_TYPES;
  readonly incomeCategories = INCOME_CATEGORIES;
  readonly expenseCategories = EXPENSE_CATEGORIES;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreateTransactionModalComponent>
  ) {
    this.form = this.fb.group({
      amount: [null, [Validators.required, Validators.min(0.01)]],
      transactionType: ['', Validators.required],
      incomeCategory: [''],
      expenseCategory: [''],
      description: ['', Validators.maxLength(100)],
      date: [null, Validators.required],
    });

    this.form.get('transactionType')?.valueChanges.subscribe((type) => {
      this.selectedTransactionType.set(type);

      const incomeControl = this.form.get('incomeCategory');
      const expenseControl = this.form.get('expenseCategory');

      if(type === 'income') {
        incomeControl?.setValidators([Validators.required]);
        expenseControl?.clearValidators();
        expenseControl?.setValue('');
      } else if(type === 'expense') {
        expenseControl?.setValidators([Validators.required]);
        incomeControl?.clearValidators();
        incomeControl?.setValue('');
      } else {
        incomeControl?.clearValidators();
        expenseControl?.clearValidators();
      }

      incomeControl?.updateValueAndValidity({ emitEvent: false })
      expenseControl?.updateValueAndValidity({ emitEvent: false })
    })
  }


  submit(): void {
    if(this.form.valid)
      this.dialogRef.close(this.form.value);
  }
  
  cancel(): void {
    this.dialogRef.close(null);
  }
}