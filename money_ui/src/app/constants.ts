const TRANSACTION_TYPES: { value: string, displayName: string }[] = [
    { value: 'income', displayName: 'Income' },
    { value: 'expense', displayName: 'Expense' }
];

const INCOME_CATEGORIES: { value: string, displayName: string }[] = [
    { value: 'salary', displayName: 'Salary' },
    { value: 'bonus', displayName: 'Bonus' },
    { value: 'investments', displayName: 'Investments' },
    { value: 'miscellaneous', displayName: 'Miscellaneous' }
];

const EXPENSE_CATEGORIES: { value: string, displayName: string }[] = [
    { value: 'rent', displayName: 'Rent' },
    { value: 'utilities', displayName: 'Utilities' },
    { value: 'groceries', displayName: 'Groceries' },
    { value: 'transportation', displayName: 'Transportation' },
    { value: 'entertainment', displayName: 'Entertainment' },
    { value: 'eatingOut', displayName: 'Eating Out' },
    { value: 'insurance', displayName: 'Insurance' },
    { value: 'miscellaneous', displayName: 'Miscellaneous' }
];

const MONTHS: { name: string, value: number}[] = [
    { name: 'January', value: 1 }, 
    { name: 'February', value: 2 }, 
    { name: 'March', value: 3 },
    { name: 'April', value: 4 }, 
    { name: 'May', value: 5 }, 
    { name: 'June', value: 6 },
    { name: 'July', value: 7 }, 
    { name: 'August', value: 8 }, 
    { name: 'September', value: 9 },
    { name: 'October', value: 10 }, 
    { name: 'November', value: 11 }, 
    { name: 'December', value: 12 }
]

export { TRANSACTION_TYPES, INCOME_CATEGORIES, EXPENSE_CATEGORIES, MONTHS };