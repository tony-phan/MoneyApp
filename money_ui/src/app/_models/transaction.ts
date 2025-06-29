export interface Transaction {
    id: number,
    amount: number,
    transactionType: string,
    incomeCategory?: string | null,
    expenseCategory?: string | null,
    description: string,
    date: string
}