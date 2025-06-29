import { Routes } from '@angular/router';
import { HomeComponent } from './components/home-page/home/home.component';
import { DashboardComponent } from './components/dashboard-page/dashboard/dashboard.component';
import { TransactionListComponent } from './components/transactions-page/transaction-list/transaction-list.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'transactions', component: TransactionListComponent }
];
