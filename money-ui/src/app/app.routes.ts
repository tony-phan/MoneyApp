import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TransactionListComponent } from './transactions/transaction-list/transaction-list.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'transactions', component: TransactionListComponent }
];
