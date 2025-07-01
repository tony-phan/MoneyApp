import { Component } from '@angular/core';
import { MaterialModule } from '../../../material.module';

@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [MaterialModule],
  templateUrl: './transaction-form.component.html',
  styleUrl: './transaction-form.component.css'
})
export class TransactionFormComponent {

}
