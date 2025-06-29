import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService)
  private baseUrl = 'https://localhost:7000/api/Transaction';

  constructor() { }

}
