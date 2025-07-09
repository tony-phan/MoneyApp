import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private baseUrl = 'https://localhost:7000/api/Transaction';

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
}
