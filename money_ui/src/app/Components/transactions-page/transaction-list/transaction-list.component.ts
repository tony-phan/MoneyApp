import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { TransactionService } from '../../../services/transaction.service';
import { MaterialModule } from '../../../material.module';
import { Transaction } from '../../../_models/transaction';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.css'
})
export class TransactionListComponent implements OnInit {
  private transactionService = inject(TransactionService);
  private accountService = inject(AccountService);

  transactions: Transaction[] = [];
  selectedTransaction: Transaction | null = null;
  
  ngOnInit(): void {
    let userId = this.accountService.currentUser()?.userId ?? '';
    this.accountService.getUserTransactions(userId).subscribe({
      next: (response) => {
        this.transactions = response;
        console.log('transactions: ', this.transactions);
      },
      error: (error) => console.log('error: ', error)
    });
  }
}
