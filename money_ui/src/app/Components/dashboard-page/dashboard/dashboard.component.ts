import { Component, inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AccountService } from '../../../services/account.service';
import { TransactionHistoryService } from '../../../services/transaction-history.service';
import { TransactionService } from '../../../services/transaction.service';
import { TransactionHistory } from '../../../_models/transaction-history';
import { MatExpansionModule } from '@angular/material/expansion';
import { MaterialModule } from '../../../material.module';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatDialogModule, MatExpansionModule, MaterialModule, MatIcon],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private dialog = inject(MatDialog);
  private accountService = inject(AccountService);
  private transactionHistory = inject(TransactionHistoryService);
  private transactionService = inject(TransactionService);

  transactionHistories: TransactionHistory[] = [];
  selectedTransactionHistory: TransactionHistory | null = null;

  ngOnInit(): void {
    var userId = this.accountService.currentUser()?.userId ?? '';
    this.accountService.getTransactionHistories(userId).subscribe({
      next: response => {
        this.transactionHistories = response;
        console.log('transactionHistories: ', this.transactionHistories);
      },
      error: error => {
        console.log('error: ', error);
      }
    });
  }

  openCreateModal() {
    console.log('openCreateModal clicked');
  }

  editHistory() {
    console.log('editHistory clicked');
  }

  deleteHistory() {
    console.log('edleteHistory clicked');
  }
}
