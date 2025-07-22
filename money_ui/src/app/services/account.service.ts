import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../_models/user';
import { Transaction } from '../_models/transaction';
import { TransactionHistory } from '../_models/transaction-history';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl = environment.apiBaseUrl + '/Account';
  currentUser = signal<User | null>(null);

  constructor(
    private http: HttpClient
  ) {}

  getAuthHeaders(): HttpHeaders {
    const token = this.currentUser()?.token;
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  login(payload: any) {
    return this.http.post<User>(this.baseUrl + '/login', payload).pipe(
      map(user => {
        if (user) {
          this.currentUser.set(user);
          localStorage.setItem('user', JSON.stringify(user))
        }
        return user;
      })
    );
  }

  logout() {
    this.currentUser.set(null);
    localStorage.removeItem('user');
  }

  register(payload: any) {
    return this.http.post<User>(this.baseUrl + '/register', payload).pipe(
      map(user => {
        if (user) {
          console.log('successfully registered user: ', user);
        }
        return user;
      })
    );
  }

  getUserTransactions(userId: string): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(this.baseUrl + `/${userId}/transactions`, { headers: this.getAuthHeaders() });
  }

  getTransactionHistories(userId: string): Observable<TransactionHistory[]> {
    return this.http.get<TransactionHistory[]>(this.baseUrl + `/${userId}/transactionHistories`, { headers: this.getAuthHeaders() });
  }
}
