import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Transaction } from '../../_models/transaction';
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-transactions-table',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transactions-table.component.html',
  styleUrl: './transactions-table.component.css'
})
export class TransactionsTableComponent {
  @Input() transactions: Transaction[] = [];
  @Output() rowClicked = new EventEmitter<Transaction>();

  columns: string[] = ['date', 'amount', 'type', 'category', 'description'];

  getCategory(transaction: Transaction): string {
    return transaction.transactionType === "Income" ? transaction.incomeCategory ?? '' : transaction.expenseCategory ?? '';
  }
}
