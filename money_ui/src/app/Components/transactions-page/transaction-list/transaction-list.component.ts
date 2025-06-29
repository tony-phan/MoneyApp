import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { TransactionService } from '../../../services/transaction.service';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.css'
})
export class TransactionListComponent implements OnInit {
  transactionService = inject(TransactionService);
  private accountService = inject(AccountService);
  
  ngOnInit(): void {
    let userId = this.accountService.currentUser()?.userId ?? '';
    this.accountService.getUserTransactions(userId).subscribe({
      next: (response) => {
        console.log('response: ', response);
      },
      error: (error) => console.log('error: ', error)
    });
  }
}
