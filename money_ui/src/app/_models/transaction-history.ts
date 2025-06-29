import { Transaction } from "./transaction";

export interface TransactionHistory {
    id: number,
    userId: string, 
    month: number,
    year: number,
    totalIncome: number, 
    totalExpenses: number,
    netBalance: number,
    transactions: Transaction[]
}