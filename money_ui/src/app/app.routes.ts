import { Routes } from '@angular/router';
import { DashboardComponent } from './Components/dashboard/dashboard.component';
import { TransactionListComponent } from './Components/transaction-list/transaction-list.component';
import { HomeComponent } from './Components/home/home.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'transactions', component: TransactionListComponent }
];
