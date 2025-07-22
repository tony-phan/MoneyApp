import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { TransactionService } from './transaction.service';
import { Observable } from 'rxjs';
import { TransactionHistory } from '../_models/transaction-history';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransactionHistoryService {
  private baseUrl = environment.apiBaseUrl + '/TransactionHistory';

  constructor(
    private http: HttpClient, 
    private accountService: AccountService, 
    private transactionService: TransactionService
  ) {}

  getAuthHeaders(): HttpHeaders {
    const token = this.accountService.currentUser()?.token;
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  delete(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.baseUrl + `/${id}`, { headers: this.getAuthHeaders() });
  }

  create(payload: any): Observable<TransactionHistory> {
    return this.http.post<TransactionHistory>(this.baseUrl + '/create', payload, { headers: this.getAuthHeaders() });
  }
}
