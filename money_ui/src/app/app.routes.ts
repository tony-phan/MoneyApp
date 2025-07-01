import { Routes } from '@angular/router';
import { HomeComponent } from './components/home-page/home/home.component';
import { DashboardComponent } from './components/dashboard-page/dashboard/dashboard.component';
import { TransactionListComponent } from './components/transactions-page/transaction-list/transaction-list.component';
import { authGuard } from './auth.guard';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
    { path: 'transactions', component: TransactionListComponent, canActivate: [authGuard] }
];
