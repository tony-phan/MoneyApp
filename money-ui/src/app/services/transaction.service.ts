import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { Observable } from 'rxjs';
import { Transaction } from '../_models/transaction';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private http = inject(HttpClient);
  private authService = inject(AccountService)
  baseUrl = 'https://localhost:7000/api/Transaction';

  constructor() { }

  getUserTransactions(userId: string): Observable<Transaction[]> {
    const token = this.authService.currentUser()?.token;
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Transaction[]>(this.baseUrl + `/transactions?userId=${userId}`, { headers });
  }
}
