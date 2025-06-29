import { Component, inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TransactionFormComponent } from '../transaction-form/transaction-form.component';
import { AccountService } from '../../../services/account.service';
import { TransactionHistoryService } from '../../../services/transaction-history.service';
import { TransactionService } from '../../../services/transaction.service';
import { Transaction } from '../../../_models/transaction';
import { TransactionHistory } from '../../../_models/transaction-history';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatDialogModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  private dialog = inject(MatDialog);
  private accountService = inject(AccountService);
  private transactionHistory = inject(TransactionHistoryService);
  private transactionService = inject(TransactionService);

  transactionHistories: TransactionHistory[] = [];
  selectedHistory: TransactionHistory | null = null;

  ngOnInit(): void {
    
  }


  openTransactionForm(transaction?: Transaction) {
    const dialogRef = this.dialog.open(TransactionFormComponent, {
      width: '400px',
      data: transaction || null
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result) {
        console.log('test');
      }
    });
  }
}
