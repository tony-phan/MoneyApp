import { Component } from '@angular/core';
import { MaterialModule } from '../../../material.module';

@Component({
  selector: 'app-transaction-history-list',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-history-list.component.html',
  styleUrl: './transaction-history-list.component.css'
})
export class TransactionHistoryListComponent {

}
