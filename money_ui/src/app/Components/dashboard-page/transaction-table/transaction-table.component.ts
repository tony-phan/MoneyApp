import { Component, Input } from '@angular/core';
import { MaterialModule } from '../../../material.module';

@Component({
  selector: 'app-transaction-table',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-table.component.html',
  styleUrl: './transaction-table.component.css'
})
export class TransactionTableComponent {

}
