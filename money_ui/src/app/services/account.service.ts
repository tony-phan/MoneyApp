import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, model, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../_models/user';
import { Transaction } from '../_models/transaction';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = 'https://localhost:7000/api/Account';
  currentUser = signal<User | null>(null);

  constructor() { }

  login(payload: any) {
    return this.http.post<User>(this.baseUrl + '/login', payload).pipe(
      map(user => {
        if (user) {
          this.currentUser.set(user);
          localStorage.setItem('user', JSON.stringify(user))
        }
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
          this.currentUser.set(user);
          localStorage.setItem('user', JSON.stringify(user))
        }
        return user;
      })
    );
  }

  getUserTransactions(userId: string): Observable<Transaction[]> {
    const token = this.currentUser()?.token;
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.http.get<Transaction[]>(this.baseUrl + `/${userId}/transactions`, { headers });
  }
}
