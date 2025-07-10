import { Component, computed, EventEmitter, Input, Output, signal, ViewChild } from '@angular/core';
import { Transaction } from '../../_models/transaction';
import { MaterialModule } from '../../material.module';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

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

  pagination = signal({ pageIndex: 0, pageSize: 8 });
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  totalPages = computed(() => {
    Math.ceil(this.transactions.length / this.pagination().pageSize)
  });
  paginatedTransactions = computed(() => {
    const {pageIndex, pageSize} = this.pagination();
    const start = pageIndex * pageSize;
    return this.transactions.slice(start, start + pageSize);
  });

  columns: string[] = ['date', 'amount', 'type', 'category', 'description'];

  constructor() {
    this.pagination.set({
      pageIndex: 0,
      pageSize: this.pagination().pageSize
    });
  }

  onPageChange(event: PageEvent): void {
    this.pagination.set({
      pageIndex: event.pageIndex,
      pageSize: event.pageSize
    });
  }

  getCategory(transaction: Transaction): string {
    return transaction.transactionType === "Income" ? transaction.incomeCategory ?? '' : transaction.expenseCategory ?? '';
  }

  // format date to MM/DD/YYYY
  formatDate(date: Date): string {
    return `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getDate().toString().padStart(2, '0')}/${date.getFullYear()}`;
  }
}
