import { Component, computed, effect, OnInit, signal, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '../../../services/account.service';
import { TransactionHistoryService } from '../../../services/transaction-history.service';
import { TransactionService } from '../../../services/transaction.service';
import { TransactionHistory } from '../../../_models/transaction-history';
import { MaterialModule } from '../../../material.module';
import { TransactionHistoryModalComponent } from '../transaction-history-modal/transaction-history-modal.component';
import { MatPaginator, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  // Signals for UI state
  readonly transactionHistories = signal<TransactionHistory[]>([]);
  readonly loading = signal<boolean>(true);
  readonly pagination = signal({ pageIndex: 0, pageSize: 8 });

  monthMapping = new Map<number, string>([
    [1, "January"],
    [2, "February"],
    [3, "March"],
    [4, "April"],
    [5, "May"],
    [6, "June"],
    [7, "July"],
    [8, "August"],
    [9, "September"],
    [10, "October"],
    [11, "November"],
    [12, "December"],
  ]);

  // MatPaginator reference
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  // Derived signals
  readonly totalPages = computed(() =>
    Math.ceil(this.transactionHistories().length / this.pagination().pageSize)
  );
  readonly totalIncome = computed(() => this.transactionHistories().reduce((sum, tH) => sum += tH.totalIncome, 0));
  readonly totalExpenses = computed(() => this.transactionHistories().reduce((sum, tH) => sum += tH.totalExpenses, 0));
  readonly netBalance = computed(() => this.transactionHistories().reduce((sum, tH) => sum += tH.netBalance, 0));

  readonly paginatedTransactionHistories = computed(() => {
    const { pageIndex, pageSize } = this.pagination();
    const start = pageIndex * pageSize;
    return this.transactionHistories().slice(start, start + pageSize);
  });

  constructor(
    private transactionHistoryService: TransactionHistoryService,
    private accountService: AccountService,
    private transactionService: TransactionService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.fetchTransactionHistories();
  }

  ngAfterViewInit(): void {
    effect(() => {
      // Reset to first page when data changes
      this.transactionHistories();
      this.pagination.set({
        pageIndex: 0,
        pageSize: this.pagination().pageSize,
      });
    });
  }

  onPageChange(event: PageEvent): void {
    this.pagination.set({
      pageIndex: event.pageIndex,
      pageSize: event.pageSize,
    });
  }

  fetchTransactionHistories(): void {
    this.loading.set(true);
    const userId = this.accountService.currentUser()?.userId ?? '';

    this.accountService.getTransactionHistories(userId).subscribe({
      next: (response) => {
        this.transactionHistories.set(response);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Failed to load histories:', error);
        this.loading.set(false);
      },
    });
  }

  deleteHistory(id: number): void {
    this.transactionHistoryService.delete(id).subscribe({
      next: () =>
        this.transactionHistories.update((list) =>
          list.filter((h) => h.id !== id)
        ),
      error: (error) => console.error('Failed to delete history:', error),
    });
  }

  openTransactions(history: TransactionHistory): void {
    this.dialog.open(TransactionHistoryModalComponent, {
      width: '600px',
      data: history.transactions,
    });
  }
}
