import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { TransactionService } from './transaction.service';

@Injectable({
  providedIn: 'root'
})
export class TransactionHistoryService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  private transactionService = inject(TransactionService);
  private baseUrl = 'https://localhost:7000/api/TransactionHistory';

  constructor() { }
}
