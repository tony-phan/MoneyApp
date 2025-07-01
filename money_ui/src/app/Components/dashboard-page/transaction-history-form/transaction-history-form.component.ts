import { Component } from '@angular/core';
import { MaterialModule } from '../../../material.module';

@Component({
  selector: 'app-transaction-history-form',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-history-form.component.html',
  styleUrl: './transaction-history-form.component.css'
})
export class TransactionHistoryFormComponent {

}
