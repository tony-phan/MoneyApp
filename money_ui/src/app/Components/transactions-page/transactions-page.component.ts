import { Component, OnInit, signal } from '@angular/core';
import { Transaction } from '../../_models/transaction';
import { TransactionService } from '../../services/transaction.service';
import { AccountService } from '../../services/account.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateTransactionModalComponent } from '../create-transaction-modal/create-transaction-modal.component';
import { SelectedTransactionModalComponent } from '../selected-transaction-modal/selected-transaction-modal.component';
import { TransactionsTableComponent } from "../transactions-table/transactions-table.component";
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-transactions-page',
  standalone: true,
  imports: [TransactionsTableComponent, MaterialModule],
  templateUrl: './transactions-page.component.html',
  styleUrl: './transactions-page.component.css'
})
export class TransactionsPageComponent implements OnInit {
  transactions = signal<Transaction[]>([]);

  constructor(
    private transactionService: TransactionService, 
    private accountService: AccountService,
    private dialog: MatDialog
  ) {}
  
  ngOnInit(): void {
    this.loadTransactions();
  }            

  loadTransactions(): void {
    var userId = this.accountService.currentUser()?.userId;
    if(!userId) return;

    this.accountService.getUserTransactions(userId).subscribe({
      next: data => {
        data = data.map((transaction) => {
          return {
            ...transaction,
            date: new Date(transaction.date)
          }
        });
        this.transactions.set(data)
      },
      error: error => console.log('Error loading transactions: ', error)
    });
  }

  openCreateModal(): void {
    const dialogRef = this.dialog.open(CreateTransactionModalComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe((result) => {
      if(result === 'refresh') 
        this.loadTransactions();
    })
  }

  onTransactionSelected(transaction: Transaction): void {
    const dialogRef = this.dialog.open(SelectedTransactionModalComponent, {
      width: '400px',
      data: transaction
    });

    dialogRef.afterClosed().subscribe((result) => {
      if(result === 'refresh') 
        this.loadTransactions();
    })
  }
}
