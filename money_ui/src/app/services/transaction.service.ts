import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { Observable } from 'rxjs';
import { Transaction } from '../_models/transaction';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private baseUrl = environment.apiBaseUrl + '/Transaction';

  constructor(
    private http: HttpClient, 
    private accountService: AccountService
  ) {}

  getAuthHeaders(): HttpHeaders {
    const token = this.accountService.currentUser()?.token;
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  create(payload: any): Observable<Transaction> {
    return this.http.post<Transaction>(this.baseUrl + '/create', payload, { headers: this.getAuthHeaders() });
  }
}
