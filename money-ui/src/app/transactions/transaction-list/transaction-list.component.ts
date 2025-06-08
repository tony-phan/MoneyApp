import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from "../../navbar/navbar.component";
import { TransactionService } from '../../services/transaction.service';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [NavbarComponent],
  templateUrl: './transaction-list.component.html',
  styleUrl: './transaction-list.component.css'
})
export class TransactionListComponent implements OnInit {
  transactionService = inject(TransactionService);
  authService = inject(AccountService);
  
  ngOnInit(): void {
    let userId = this.authService.currentUser()?.userId ?? '';
    this.transactionService.getUserTransactions(userId).subscribe({
      next: (response) => {
        console.log('response: ', response);
      },
      error: (error) => console.log('error: ', error)
    });
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    this.authService.currentUser.set(user);
  }
}
